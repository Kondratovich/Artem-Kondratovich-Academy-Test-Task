# Artem-Kondratovich-Academy-Test-Task
Implementation of Dijkstra's algorithm for finding the best path
## Task

You are an engineer designing drone rovers. You need to design the rover's path over known terrain to maximize power savings.

### Terrain
You have received data about the area in a coded form: a photograph converted into a matrix from a number. One matrix is ​​a rectangular x-by-y meter image. Here is an example of one such converted photo, it shows a shot of 100 by 100 meters: <br>
Photo 1: <br>
0 2 3 4 1 <br>
2 3 4 4 1 <br>
3 4 5 6 2 <br>
4 5 6 7 1 <br>
6 7 8 7 1 <br>

The numbers indicate the level above sea level. 0 is the height exactly at sea level, and for example 4 is 4 units above sea level. Photo 1 encoded a hill that is shallow on the left and abruptly drops off on the right.

A small mound would look like this <br>
Photo 2: <br>
0 1 1 1 0 <br>
1 1 3 1 1 <br>
0 1 1 1 0 <br>
0 0 0 0 0 <br>

And like this: a hollow between two hills <br>
Photo 3: <br>
1 1 2 3 4 <br>
1 0 1 2 3 <br>
2 1 1 1 2 <br>
3 3 1 0 1 <br>
4 3 1 1 0 <br>

This data shows a rock or a ravine, as there is a very sharp elevation difference in the middle of the image <br>
Photo 4: <br>
1 1 6 7 7 <br>
1 1 6 7 8 <br>
1 6 7 8 9 <br>

And on this is a small hole <br>
Photo 5: <br>
3 4 4 4 4 3 <br>
3 2 1 1 1 4 <br>
4 2 1 1 3 4 <br>
4 4 2 2 3 4 <br>

The data will come to you as a matrix with non-negative numbers. Matrix size NxM.

### Rover
The rover always moves from the top left point [0] [0] to the bottom right point [N - 1] [M - 1], where N and M are the length and width of the matrix. This is necessary in order to cut the photo into identical pieces, process them separately, and then glue them all the way.

Your rover has several limitations:
1. Movement <br>
From any point, the rover can only move in four directions: north, south, west, east. The rover cannot drive diagonally - this feature has not yet been implemented. The rover cannot return to the point at which it already was.
2. Charge <br>
The rover runs on a charge. You know that it is very expensive for a rover to get up and down. He spends a unit of charge on the movement itself, and additional units on ascent and descent. Rover would generally live quietly if he drove on the asphalt in Belarus, then he would have spent a linear charge and wouldn’t blow his mustache, but his life turned out differently.
3. Charge consumption <br>
The charge is consumed according to the rule: <br>
For 1 step, the rover always spends 1 unit of charge. The rover uses a charge proportional to the difficulty of the ascent or descent to climb or descend. The difficulty of climbing or descending is the difference between the heights. <br>

For example, in such an area <br>
1 2 <br>
1 5 <br>
on the way from [0] [0] to [0] [1], the rover will spend 2 units of charge: 1 unit of charge for the movement itself, and another 1 unit of charge for the ascent to [0] [1]. And from [0] [1] to [1] [1] the rover will spend 4 units of charge: 1 unit for the movement itself, and 3 units (5 - 2) for the rise <br>

You need to calculate the rover's path from the top-left [0] [0] point to the bottom-right [N - 1] [M - 1] point with the least amount of charge. <br>
You do not know in advance the size of the photo that you will process, N and M are arbitrary non-negative numbers. <br>

### Plan
Make a route plan and projected expense in a txt file. Name the file path-plan.txt <br>
For photography <br>
0 4 <br>
1 3 <br>
the plan will be like this: <br>

path-plan.txt <br>
[0] [0] -> [1] [0] -> [1] [1] <br>
steps: 2 <br>
fuel: 5 <br>

The rover goes from 0 to 1 to 3, takes two steps, spends 5 charges. If he went first at 4, then at 3, he would have taken the same number of steps, but would have spent 7 charges. Optimal path: 2 steps and 5 charges.

If there are several paths on the map, choose any of them.
