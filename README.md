# MockService

run mb server **mb start --port 2525** in terminal

## Let's consider the basic scenarios for working with the Mountebank service. 
We will create an imposter and use it to model the behavior of the service under test in order to get the result we need.
When we make an HTTP request to the given URL, we plan to see that the response body contains the given message and status code:
1. for the path **/home** we will see the message **"Welcome to our store!"** and the status code **200**
2. for the path **/settings** we will see the message **"You need to login before configure account settings"** and the status code **401**
3. for the path **/signup** we will see the message **"Oops! Time has expired. Please check the connection"** and the status code **408**
