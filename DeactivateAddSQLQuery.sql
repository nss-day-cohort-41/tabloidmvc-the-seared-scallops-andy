/*
//This is the Alter required on UserProfile to implement deactivation of users

ALTER TABLE UserProfile
	ADD IdIsActive Integer

	//Run this update to set all of your users to active (1 is deactivated)

	UPDATE UserProfile
		SET IdIsActive = 0

*/

SELECT * FROM UserProfile

INSERT INTO UserProfile
       (DisplayName, FirstName, LastName, Email, CreateDateTime, ImageLocation, UserTypeId, IdIsActive)
      OUTPUT INSERTED.Id
     VALUES('DougThePug', 'Doug', 'Pug', 'Pug@gmail.com', 2020-09-27, null, 2, 1)
