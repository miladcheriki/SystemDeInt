Hey there! This project is my take on the interview design, showcasing a microservices architecture with a few neat features. Here's what I built and how it works:

What's Inside?

Structure
Additional services in the cluster, such as:
    API Gateway: Built with YARP, it handles routing between services.
    Identity Manager: Used to generate tokens for your API requests.
    
Lead API Service
This microservice lets you fetch leads (or a single lead) from a SQL database. Here's what you can do with it:

    Get a list of leads with optional filters:
    Search by a term (like% behavior for fields like email or names).
    Sort results by ascending or descending order.
    Supports caching, You can choose between in-memory or Redis caching by updating the CacheSettings in appsettings.json.

HTTP Logging
I’ve used Serilog to log all incoming HTTP requests and responses into a file. Simple, reliable, and easy to tweak!
The logging setup is straightforward, but feel free to tweak it if needed!

Testing Made Easy
Each project includes an .http file that you can use to test the APIs directly with tools like REST Client or Postman.
