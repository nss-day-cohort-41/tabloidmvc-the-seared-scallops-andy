--SELECT * FROM Comment
SELECT Id, PostId, UserProfileId, Subject, Content, CreateDateTime 
FROM Comment
WHERE PostId = 2


--INSERT INTO [Comment] (
--	[Id], [PostId], [UserProfileId], [Subject], [Content], [CreateDateTime])
--VALUES (
--	3, 2, 1, 'is it really though?', 
--'I think it definitely is.',SYSDATETIME());
--SET IDENTITY_INSERT [Comment] ON

--SELECT * FROM Post