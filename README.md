# SOMIOD

## Setting up the database

- Install PostgreSQL on your system.
- Postgres requires the user `postgres` to be the session user to use the postgres command line. To do that, run the command `sudo -i -u postgres` to enter a session with the postgres user.
- Run the command `psql` to enter the postgres command line. 
- Create a user ` CREATE USER projeto_is CREATEDB LOGIN PASSWORD 'password';`. This will create a user named "projeto_is" with the password "password".
- Create a database, named "projeto_is", owned by the recently created user, also named "projeto_is": `CREATE DATABASE projeto_is OWNER projeto_is ENCODING UTF8 LC_COLLATE='pt_PT.UTF8' LC_CTYPE='pt_PT.UTF8' TEMPLATE template0;`.
- Import the `latest.sql` dump into the database you just created.