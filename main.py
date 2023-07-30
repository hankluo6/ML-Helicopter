from mlagents_envs.base_env import ActionTuple
from mlagents_envs.environment import UnityEnvironment
from mlagents.trainers import demo_loader as dl
from sklearn.ensemble import RandomForestRegressor
import numpy as np
import pickle

TRAINING = True
DEMO_PATH = 'demostration path'
SEED = 42

if __name__ == '__main__':
    
    if TRAINING:
        # get demo file datas
        behavior_spec, demo_buffer = dl.demo_to_buffer(DEMO_PATH, 1) 
        observation_length = []

        for i in behavior_spec[0]:
            observation_length.append(i.shape) # Collect observation size

        action_length = behavior_spec[1][0]

        buffer_keys = list(demo_buffer.keys())
        buffer_len = len(demo_buffer[buffer_keys[2]])
        n_obs_type = len(observation_length)
        # demo_buffer stores value from second key to n_obs_type
        X = np.hstack([np.array(demo_buffer[buffer_keys[2 + idx]]).reshape(buffer_len, -1) for idx in range(n_obs_type)])
        Y = np.array(demo_buffer[buffer_keys[2 + n_obs_type]])

        np.save("helicopter_X.npy", X)
        np.save("helicopter_Y.npy", Y)

        print(X.shape)

        # training
        clf = RandomForestRegressor(random_state=42)
        clf.fit(X, Y)
        # save model
        with open('clf.pickle', 'wb') as f:
            pickle.dump(clf, f)
    else:
        # load model
        with open('clf.pickle', 'rb') as f:
            clf = pickle.load(f)

        # Open environment
        # step 1: run this file and
        # step 2: when it appear "Start training by pressing the Play button in the Unity Editor.", click unity editor play button
        unity_env = UnityEnvironment(base_port=5004, seed=SEED, side_channels=[])
        brain_name = "Helicopter?team=0"
        agent_scores = 0

        for episode in range(100):
            
            rewards = 0
            
            unity_env.reset()

            # get environment steps
            decision_steps, terminal_steps = unity_env.get_steps(brain_name)

            # prepare input date
            states = decision_steps.obs
            X = np.hstack([states[idx].reshape(1, -1) for idx in range(len(states))])

            while True:
                # predict actions
                ret = np.clip(clf.predict(X), -1, 1)
                
                print(ret)
                actionAT = ActionTuple()
                actionAT.add_continuous(continuous=np.array(ret))
                unity_env.set_actions(brain_name, actionAT)

                unity_env.step()
                decision_steps, terminal_steps = unity_env.get_steps(brain_name)

                # get the next states for each unity agent in the environment
                next_states = decision_steps.obs  
                rewards = decision_steps.reward

                if len(terminal_steps) != 0:
                    break

                # set new states to current states for determining next actions
                states = next_states
                X = np.hstack([states[idx].reshape(1, -1) for idx in range(len(states))])
                
                # Update episode score for each unity agent
                agent_scores += rewards
            print(f'episode: {episode}, rewards: {rewards}')

        unity_env.close()
