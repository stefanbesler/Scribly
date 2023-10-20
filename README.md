# Scribly

This fun little project is a proof-of-concept for writing text on a XY table with a pneumatic actuator that holds a pen and two motors to move the pen.

A C# program is capturing input of a user and splits every character that is typed into segments of lines and splines. The segments are sent to a Beckhoff PLC via an ADS client and 
trajectories are calculated to control two motors.
The PLC is implemented with [Zeugwerk Framework](https://doc.zeugwerk.dev/) and the trajectories are calculated by [Struckig](https://github.com/stefanbesler/struckig).

![Peek 2023-10-20 23-26](https://github.com/stefanbesler/Scribly/assets/84121166/a71f8275-c14c-419d-b1c5-aeb397b5e950)
