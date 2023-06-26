SELECT U.UserName 'User Name',R.Name 'Role Name'
FROM IdentityDemo.dbo.AspNetRoles R
JOIN IdentityDemo.dbo.AspNetUserRoles UR
	ON R.Id = UR.RoleId
JOIN IdentityDemo.dbo.AspNetUsers U
	ON U.Id = UR.UserId