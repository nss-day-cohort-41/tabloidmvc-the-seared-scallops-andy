/*
//This is the Alter required on UserProfile to implement deactivation of users

ALTER TABLE UserProfile
	ADD IdIsActive Integer

	//Run this update to set all of your users to active (1 is deactivated)

	UPDATE UserProfile
		SET IdIsActive = 0

*/

SELECT * FROM UserType

