# PixelatedCamera
Render to a RenderTexture and still have the new unity event system map the pointer input correctly.
Tested only with unity 6000.6.

# How To Use
1. Add the PixelCamera script to your project and add it on your camera.
1. Create a RenderTexture and set it to the PixelCamera.RenderTexture refference.
1. Create a Canvas with a RawImage and set the texture to your RenderTexture to display it to the screen.
1. You can make the canvas scale with screen size and set the RawImage to stretch over the screen to fill it completely.
