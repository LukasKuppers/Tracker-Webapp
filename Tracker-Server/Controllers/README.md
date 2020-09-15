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
	"SessionId": "asdf-qwer-zxcv-uiop"
}
```

#### Response Codes:
- **422** if the request body is malformed (null fields, ect...)
- **400** if the Email or Password is an empty string
- **401** if the given login credentials are not valid
- **200** if the given login credentials are valid
