This project was used to create a test environment for rendering instanced 3D models using vertex animations.
The goal was to create a tool that can render thousands of animated objects without abolishing the frame rate.
This can be achieved by baking the the vertex positions for each frame of the animation into a texture, which is used by the vertex shader to "animate" the object at runtime.
By using this approach it is possible to render ten thousand animated objeccts with 661 vertices at a frame rate of 153 (AMD Ryzen 9 3900X/NVIDIA GeForce RTX 2080 SUPER).

![ThousandsOfAnimatedSpiders](https://github.com/F4bj4ck/GPUInstancingTest/assets/36537405/19e07f56-bdc4-46c1-9f49-2901116f6807)
