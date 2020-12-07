# API Documentation
NOTE: very early specification

A comprehensive list of all the endpoints provided by the back end. Endpoints should only be accesible if the client sends 
their session ID in the authorization header. The session ID is provided by the server in the request body of the
`PUT /api/authorization` call, if the provided credentials are valid.

### Authorization API:
`Get /api/authorization`

Asks the server if the user is authorized. No JSON body is required, but a valid user will have their correct session ID in the requests auth header.
If the user is authenticated the request returns a JSON body of the form:

```
{
	"Role": "user"
}
```

Where the role is the level of security the user has obtained. Only two roles exist: {user, admin}

#### Response Codes:
- **400** if the sessionID cookie doesn't exist, or is malformed
- **401** if the sessionID isn't valid
- **200** and the user role `user` or `admin` if the sessionID is valid

___
`PUT /api/authorization`

Accepts a JSON body in the form:
```
{
	"Email": "user@example.com",
	"Password": "mysecretpassword" 
}
```
which should be the login details for a user.
If the request is successful, it will return the string representation of the users new Session ID. (The session ID should be added as a cookie for making subsequent api calls with the key 'sessionID')

example output:
```
{
	"SessionId": "3dd272c5-3a79-4b33-a3dd-216fcae8629d"
}
```

#### Response Codes:
- **400** if the Email or Password is null or an empty string
- **401** if the given login credentials are not valid
- **200** if the given login credentials are valid - returns session ID

___
`POST /api/authorization`

Allows user to register new account
Accepts a JSON body in the form:
```
{
	"Username": "Reinhold",
	"Email": "reinhold.messner@example.com",
	"Password": "Reinhold123"
}
```

This should be the registration info for a new user.
Note: password/ email validation have not *yet* been implemented on the BE.
If the request is successful, it will return the same JSON body as in the request.

#### Response Codes:
- **400** if any of the provided fields are null or empty
- **422** if the email or password was invalid (malformed email, password doesn't meet requirements)
- **409** if a user with the specified Email Address already exists
- **201** if the registration was successful - returns user info

### Users API:
`GET /api/users/current`

Get a the current user info. Note that this requires that the user is logged in with a valid session, where the server-provided sessionID is provided in the request header - goto top of docs to see sessionID requirements.

If successful, a JSON body of the following form is returned:
```
{
	"id": "854b1852-4ae6-4f5f-81e5-b46ef4297ce4"
	"username": "Jon Jones", 
	"email": "JonnyJones@hotmail.com", 
	"projects": [
		"7db7d2b7-6d33-4123-bc0c-a322e35adc13", 
		"3dd272c5-3a79-4b33-a3dd-216fcae8629df"]
}
```
Note that the Projects is a list of project ID's

#### Response Codes:
- **400** if the sessionID is empty or malformed
- **401** if the sessionID is invalid
- **200** if the request was successful and the user info is returned in the session body

___
`GET /api/users/{userId}`

Get info for the specified user. This is exposes public information that every user can access (given the correct userID). 

If successful, a JSON body of the following form is returned:
```
{
	"id": "854b1852-4ae6-4f5f-81e5-b46ef4297ce4"
	"username": "Jon Jones", 
	"email": "JonnyJones@hotmail.com", 
	"projects": [
		"7db7d2b7-6d33-4123-bc0c-a322e35adc13", 
		"3dd272c5-3a79-4b33-a3dd-216fcae8629df"]
}
```

#### Response Codes:
- **400** if the userId is empty or malformed
- **404** if no user with the specified Id exists
- **200** if the request was successful and the users info was returned

### Projects API:
`GET /api/projects/{projectId}`

Get a specific project by its Id. The user must own the project or be a member of the project.
Does not require a JSON body.

Example output for a valid request:
```
{
	"Id": "7db7d2b7-6d33-4123-bc0c-a322e35adc13", 
	"Title": "My Project", 
	"DateCreated": "01/10/2020", 
	"Owner": "Melissa", 
	"Members": [], 
	"Tasks": []
}
```

The `Members` and `Tasks` properties should be lists containing user ID's and task ID's, respectively.

#### Response Codes:
- **400** if the given projectID is empty or malformed
- **403** if the user is not the owner or a member of the project
- **404** if no project exists with the given projectId
- **401** if the user is not logged in
