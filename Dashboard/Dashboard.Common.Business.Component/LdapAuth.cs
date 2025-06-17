using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using Dashboard.Common.Business.Component;

public class LdapAuthentication
{
    private string _path;
    private string _filterAttribute;

    public LdapAuthentication(string path)
    {
        _path = path;
    }

    public bool RegisterNewUser(string domain, string username, string password, string email)
    {
        string domainAndUsername = domain + @"\" + username;
        DirectoryEntry ouEntry = new DirectoryEntry(_path);

        using (var pc = new PrincipalContext(ContextType.Domain, domain))
        {
            using (var up = new UserPrincipal(pc))
            {
                up.SamAccountName = username;
                up.EmailAddress = email;
                up.SetPassword(password);
                up.Enabled = true;
                up.ExpirePasswordNow();
                up.Save();
            }
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    try
        //    {
        //        DirectoryEntry childEntry = ouEntry.Children.Add("CN=TestUser" + i, "user");
        //        childEntry.CommitChanges();
        //        ouEntry.CommitChanges();
        //        childEntry.Invoke("SetPassword", new object[] { "password" });
        //        childEntry.CommitChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        return true;
    }

    public bool IsAuthenticated(string domain, string username, string pwd)
    {
        string domainAndUsername = domain + @"\" + username;

        DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

        try
        {
            //Bind to the native AdsObject to force authentication.
            object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (null == result)
            {
                return false;
            }

            _path = result.Path;
            _filterAttribute = (string)result.Properties["cn"][0];


        }
        catch (Exception ex)
        {
            Logger.LogToFile("LdapAuth.log", ex);
            return false;
        }

        return true;
    }

    public string GetGroups()
    {
        DirectorySearcher search = new DirectorySearcher(_path);
        search.Filter = "(cn=" + _filterAttribute + ")";
        search.PropertiesToLoad.Add("memberOf");
        StringBuilder groupNames = new StringBuilder();

        try
        {
            SearchResult result = search.FindOne();
            int propertyCount = result.Properties["memberOf"].Count;
            string dn;
            int equalsIndex, commaIndex;

            for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
            {
                dn = (string)result.Properties["memberOf"][propertyCounter];
                equalsIndex = dn.IndexOf("=", 1);
                commaIndex = dn.IndexOf(",", 1);
                if (-1 == equalsIndex)
                {
                    return null;
                }
                groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                groupNames.Append("|");
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile("LdapAuth.log", ex);
            throw new Exception("Error obtaining group names. " + ex.Message);
        }

        return groupNames.ToString();
    }

    public enum objectClass
    {
        user, group, computer
    }

    public enum returnType
    {
        distinguishedName, ObjectGUID
    }

    public string GetObjectDistinguishedName(objectClass objectCls, returnType returnValue, string objectName, string LdapDomain)
    {
        string distinguishedName = string.Empty;
        string connectionPrefix = "LDAP://" + LdapDomain;
        DirectoryEntry entry = new DirectoryEntry(connectionPrefix);
        DirectorySearcher mySearcher = new DirectorySearcher(entry);

        switch (objectCls)
        {
            case objectClass.user:
                mySearcher.Filter = "(&(objectClass=user)(|(cn=" + objectName + ")(sAMAccountName=" + objectName + ")))";
                break;
            case objectClass.group:
                mySearcher.Filter = "(&(objectClass=group)(|(cn=" + objectName + ")(dn=" + objectName + ")))";
                break;
            case objectClass.computer:
                mySearcher.Filter = "(&(objectClass=computer)(|(cn=" + objectName + ")(dn=" + objectName + ")))";
                break;
        }
        SearchResult result = mySearcher.FindOne();

        if (result == null)
        {
            throw new NullReferenceException("Unable to locate the distinguishedName for the object " + objectName + " in the " + LdapDomain + " domain");
        }
        DirectoryEntry directoryObject = result.GetDirectoryEntry();
        if (returnValue.Equals(returnType.distinguishedName))
        {
            distinguishedName = "LDAP://" + directoryObject.Properties["distinguishedName"].Value;
        }
        if (returnValue.Equals(returnType.ObjectGUID))
        {
            distinguishedName = directoryObject.Guid.ToString();
        }
        entry.Close();
        entry.Dispose();
        mySearcher.Dispose();
        return distinguishedName;
    }

    public bool UserExists(string username)
    {
        using (DirectorySearcher searcher = new DirectorySearcher(_path))
        {
            searcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", username);

            using (SearchResultCollection results = searcher.FindAll())
            {
                if (results.Count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}