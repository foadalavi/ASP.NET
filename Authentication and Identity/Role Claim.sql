SELECT R.Name 'Role Name', C.ClaimType, C.ClaimValue
FROM IdentityDemo.dbo.AspNetRoleClaims C
join IdentityDemo.dbo.AspNetRoles R
	On R.Id = C.RoleId