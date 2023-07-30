from moviepy.editor import VideoFileClip

# Load the video file
video_path = "resources/demo_new.mp4"
video_clip = VideoFileClip(video_path)

# Define the speed factor to speed up the video
# speed_factor = 4  # This will double the video speed

# # Speed up the video by setting the speed attribute
# speed_up_video_clip = video_clip.speedx(speed_factor)

# # Write the new video to a file
# output_path = "resources/new_demo.mp4"
# speed_up_video_clip.write_videofile(output_path)

# Define the output GIF file name
output_gif_path = "resources/result.gif"

# Write the new video as a GIF
video_clip.write_gif(output_gif_path)

# from moviepy.video.io.ffmpeg_tools import ffmpeg_extract_subclip

# ffmpeg_extract_subclip("video1.mp4", 0, 10, targetname="test.mp4")