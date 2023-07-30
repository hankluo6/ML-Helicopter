# Machine Learning Helicopter Simulator with VR

This project aims to construct a helicopter simulator environment and experiment with different machine learning algorithms to train the agent for optimal performance in the simulated environment.

## Prerequisite

This project includes below package:

- `ML Agents` for Reinforcement Learning
- `OpenVR XR Plugin` for Virtual Reality
- `Silantro Helicopter Simulator Toolkit` for helicopter controller

## Environment

The observation uses CameraSensor to capture 84x84 pixel values, stacked in sets of 4 to receive motion information.

The actions consist of 3 float values representing the degree of three directions (roll, pitch, yaw) in the helicopter.


|Observations|Actions
|---|---|
|84 x 84 x 4 |3 (Roll, Pitch, Yaw)

## Experiment

We conduct experiments with different machine learning and reinforcement learning approaches and compare their performance.

### Machine Learning

In this section, we use `scikit-learn` to design our model. We provide a framework to integrate  Python and Unity3D easily.

1. Run the agent in Unity3D with the `Demonstration Recorder` script to generate a demo file.
2. Update `demo_path` in `main.py`.
3. Run `main.py` to parse the demonstraction file and train the model.
4. Set `TRAINING` to false in `main.py` to start testing mode, and then run `main.py` and Unity Engine together to predict values in the environment.

### Reinforcement Learning

Here, we use ML Agents to train our agent in the environment. We employ Policy Proximal Optimization to improve the agent's performance. You can find detailed configuration in `helicopter.yaml`.

## Result

![result](resources/result.gif)