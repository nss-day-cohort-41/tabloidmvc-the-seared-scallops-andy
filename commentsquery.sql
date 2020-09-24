--SELECT * FROM Comment
SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime, p.Title AS PostTitle
FROM Comment c
JOIN Post p ON c.PostId = p.Id
WHERE PostId = 1
ORDER BY CreateDateTime DESC

--ORDER BY PublishDateTime DESC"

--SELECT Title FROM Post

INSERT INTO [Comment] (
	[Id], [PostId], [UserProfileId], [Subject], [Content], [CreateDateTime])
VALUES (
	3, 2, 1, 'is it really though?', 
'I think it definitely is.',SYSDATETIME());
--SET IDENTITY_INSERT [Comment] ON

--SELECT * FROM Post