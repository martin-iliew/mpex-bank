type Role = 'viewer' | 'editor' | 'admin';

const PERMISSIONS: Record<Role, string[]> = {
  viewer: ['view:post'],
  editor: ['view:post', 'create:post', 'edit:post'],
  admin: ['view:post', 'create:post', 'edit:post', 'delete:post'],
};

export const checkPermission = (
  user: { role: Role },
  action: string,
  resource: string
): boolean => {
  const permissions = PERMISSIONS[user.role];
  if (!permissions) return false;
  return permissions.includes(`${action}:${resource}`);
};

export default checkPermission;
