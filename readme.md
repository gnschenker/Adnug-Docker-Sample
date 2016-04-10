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

#Docker
Make sure you have Docker toolbox installed and the Docker Host VM called `default` is running.
Create a Docker image from you application with this command

`docker build -t adnug-sample .`

This should create an image with the name `adnug-sample` that you can see if you execute the command `docker images`.
To run the image use this command

`docker run -dt --name adnug -p 5000:5000 adnug-sample`

you can always see what's happening inside the running container by using the `docker logs` command. In our case the container is called `adnug` thus

`docker logs adnug`

should display the log of our application inside the container. Note that it takes a while after starting the container until the application is ready to run and producing any logs...

Now in you browser navigate to `192.168.99.100:5000` and enjoy the application running in a container.
If your Docker host is not using the IP address `192.168.99.100` you can find out which one it uses using this command

`docker-machine ip default`

this will return the IP address of your Docker Host. This assumes your Docker host is called `default`.

#A Stack in Docker Cloud
##Running the web site
To run the web site in the cloud we need to define a stack and in it a `Stackfile`. Our file looks like this

```
web:
  image: 'clearmeasure/adnug-docker-sample:v1'
  deployment_strategy: high_availability
  restart: always
  ports:
    - '5000:5000'
```

Note how we open port 5000 to the public to be able to access our web page from the internet.

##High Availability
To achieve high availability we need to scale out our web site. We want it to run on at least 3 nodes. But now we also need a load balancer. We can use HA Proxy for this.
Our `Stackfile` now looks like this
```
lb:
  image: 'dockercloud/haproxy:latest'
  deployment_strategy: high_availability
  links:
    - web
  ports:
    - '80:80'
  restart: always
  roles:
    - global
web:
  image: 'clearmeasure/adnug-docker-sample:v1.2'
  deployment_strategy: high_availability
  environment:
    - NAME=Gabriel
  restart: always
  target_num_containers: 3
```
Note how the load balancer service `lb` has the role `global` assigned. This allows it to use the Docker Cloud API to get information about the running instances of our web site.
Also note how we can configure environment variables. We assign a value to the `NAME` variable in the `web` service.

##Blue-Green Deployment
To achieve zero downtime we need to have non-destructive deployment (also called blue-green deployment).
Our `Stackfile` now looks like this

```
lb:
  image: 'dockercloud/haproxy:latest'
  deployment_strategy: high_availability
  links:
    - web-blue
  ports:
    - '80:80'
  restart: always
  roles:
    - global
web-blue:
  image: 'clearmeasure/adnug-docker-sample:v1.2'
  deployment_strategy: high_availability
  environment:
    - NAME=Gabriel
  restart: always
  target_num_containers: 3
web-green:
  image: 'clearmeasure/adnug-docker-sample:v1.2'
  deployment_strategy: high_availability
  environment:
    - NAME=Gabriel
  restart: always
```

Note how the load balancer service `lb` is linked to the blue version of the website.