#Introduction
This is sample code accompanying my talk about **How Docker simplifies Continuous Integration and Continuous Deployment** at the ADNUG meeting in Austin on 4/11/2016
#Instructions
Clone this repository. 
*Note: the following commands are assuming ASP.NET Core RC1. If you have a later version installed the commands will differ. Please consult the .NET documentation.*
Open a command shell and run

`dnu restore`

next run

`dnu build`

to build the solution. 
Now this sample is built for ASP.NET Core RC1 and thus we need to make sure our default runtime is the correct one. Use 

`dnvm list`

to see which one is currently the active one on your system. Most probably it is the full clr and not coreclr. To change the `default` use this command

`dnvm use 1.0.0-rc1-update1 -r coreclr -arch x64`

Using `dnvm list` again double check that the correct version is selected.
Now we can run the application using

`dnx web`

The `kestrel` web server will be started and once up and running will be listening at port 5000. Open a browser and navigate to `localhost:5000` to use the application.


