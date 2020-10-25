# API Documentation
NOTE: very early specification

A comprehensive list of all the endpoints provided by the back end. Endpoints should only be accesible if the following cookies exist:
- sessionID: A UUID provided by the authorization api

### Authorization API:
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
- **409** if a user with the specified Email Address already exists
- **201** if the registration was successful - returns user info

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
- **404** if the no project exists with the given projectId
- **401** if the user is not logged in
