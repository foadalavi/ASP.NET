SELECT U.UserName 'UserName', C.ClaimType, C.ClaimValue
FROM IdentityDemo.dbo.AspNetUserClaims C
JOIN IdentityDemo.dbo.AspNetUsers U
	ON U.Id = C.UserId