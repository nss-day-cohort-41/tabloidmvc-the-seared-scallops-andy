SELECT * FROM UserProfile

UPDATE UserProfile
		SET UserTypeId = @userType
		WHERE Id = @id