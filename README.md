# Piktiv Workshop T1 - part 2

TodoApp is a simple dotnet 8 application that stores your own personal TODO list, using Postgres and Redis for storage.

The TodoApp presents a web interface at port 8080 (so if you were running the application outside of docker, you could just browse `http://localhost:8080`)

Your mission is to:
- Package the application as a docker image
- Create a docker compose that sets up the application together with a Redis-cache and Postgres-database.
- Extra credit: The data in redis and postgresql should also survive a restart of their container, so any todos created should still be alive after a restart.

The application can be configured with the following environment variables \
REDIS_URL - The URL to the Redis instance to use \
DATABASE_HOST - The URL to the Postgres-system to use \
DATABASE_USER - The Postgres user (Postgres has a default user called postgres that you can use) \
DATABASE_PASSWORD - Whatever password you specified when setting up the postgres docker config 

- The TodoApp will automatically create the database and tables if needed, so long as it is connected to a postgres system.

## C# hints
A C# application can be built by using: \
dotnet publish -o {PathToOutput} \
Replace {PathToOutput} with wherever you want to build to and then run your application from that folder
