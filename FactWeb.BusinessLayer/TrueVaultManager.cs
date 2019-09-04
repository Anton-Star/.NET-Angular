using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Model.TrueVault;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FactWeb.BusinessLayer
{
    public class TrueVaultManager
    {
        private string apiKey;
        private const string BaseUrl = "https://api.truevault.com/v1/";
        public const string Success = "success";

        public TrueVaultManager()
        {
            this.apiKey = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.DocumentLibraryApiKey];
        }

        public UserResult Login(string userName, string password, string accountId)
        {
            var parms = new Dictionary<string, object>
            {
                {"username", userName},
                {"password", password},
                {"account_id", accountId}
            };

            var result = WebHelper.MultipartFormDataPost<UserResult>(BaseUrl + "auth/login", parms, null);

            return result;
        }

        public UserResult CreateUser(string userName, string password)
        {
            var parms = new Dictionary<string, object>
            {
                {"username", userName},
                {"password", password}
            };

            try
            {
                var result = WebHelper.MultipartFormDataPost<UserResult>(BaseUrl + "users", parms, this.apiKey);

                return result;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    return this.GetUserByUserName(userName);
                }
                else
                {
                    throw;
                }
            }

        }

        public UserResult DeleteUser(string userId)
        {
            var result = WebHelper.Delete<UserResult>(BaseUrl + "users/" + userId, this.apiKey);
            return result;
        }

        public UserResult GetUserByUserName(string userName)
        {
            var result = this.GetAllUsers();

            var user = result.Users.SingleOrDefault(x => x.Username == userName);

            if (user == null) return null;

            return new UserResult
            {
                Result = Success,
                User = user
            };
        }

        public VaultResult CreateVault(string vaultName)
        {
            var parms = new Dictionary<string, object>
            {
                {"name", vaultName}
            };
            try
            {
                var result = WebHelper.MultipartFormDataPost<VaultResult>(BaseUrl + "vaults", parms, this.apiKey);

                return result;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    return this.GetVaultByName(vaultName);
                }
                else
                {
                    throw;
                }
            }

        }

        public VaultResult DeleteVault(string vaultId)
        {
            var result = WebHelper.Delete<VaultResult>(BaseUrl + "vaults/" + vaultId, this.apiKey);
            return result;
        }

        public VaultResult GetVaultByName(string vaultName)
        {
            var result = this.GetAllVaults();
            var vault = result.Vaults.SingleOrDefault(x => x.Name == vaultName);

            if (vault == null) return null;

            return new VaultResult
            {
                Result = Success,
                Vault = vault
            };
        }

        public AllVaultResult GetAllVaults()
        {
            var result = WebHelper.Get<AllVaultResult>(BaseUrl + "vaults", this.apiKey);
            return result;
        }

        public GroupFullResult CreateGroup(string vaultId, string groupName, string userId)
        {

            var parms = new Dictionary<string, object>
            {
                {"name", groupName},
                {"policy", EncodeHelper.EncodeToBase64("[{\"Activities\": \"CRUD\", \"Resources\": [\"Vault::.*\", \"Vault::" + vaultId + "\", \"Vault::" + vaultId + "::Schema::.*\", \"Vault::" + vaultId + "::Document::.*\", \"Vault::" + vaultId + "::Blob::.*\", \"Vault::" + vaultId + "::Search::.*\"]}]") },
                {"user_ids", userId }
            };

            try
            {
                var result = WebHelper.MultipartFormDataPost<GroupFullResult>(BaseUrl + "groups", parms, this.apiKey);

                return result;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    return this.GetGroupByName(groupName);
                }
                else
                {
                    throw;
                }
            }


        }

        public GroupFullResult UpdateGroup(string groupId, string groupName, string userIds)
        {
            var parms = new Dictionary<string, object>
            {
                {"name", groupName},
                {"user_ids", userIds}
            };

            try
            {
                var result = WebHelper.Put<GroupFullResult>(BaseUrl + "groups/" + groupId, parms, this.apiKey);

                return result;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ProtocolError)
                {
                    return this.GetGroupByName(groupName);
                }
                else
                {
                    throw;
                }
            }
        }

        public GroupResult DeleteGroup(string groupId)
        {
            var result = WebHelper.Delete<GroupResult>(BaseUrl + "groups/" + groupId, this.apiKey);
            return result;
        }

        public GroupFullResult GetGroupByName(string name)
        {
            var groups = this.GetAllGroups();

            var group = groups.Groups.SingleOrDefault(x => x.Name == name);

            if (group == null) return null;

            return new GroupFullResult
            {
                Result = Success,
                Group = group
            };
        }

        public AllGroupResult GetAllGroups()
        {
            var result = WebHelper.Get<AllGroupResult>(BaseUrl + "groups", this.apiKey);

            return result;
        }

        public AllUserResult GetAllUsers()
        {
            var result = WebHelper.Get<AllUserResult>(BaseUrl + "users", this.apiKey);

            return result;
        }

        public UserResult GetAccessToken(string userId)
        {
            var parms = new Dictionary<string, object>();

            var result = WebHelper.MultipartFormDataPost<UserResult>(BaseUrl + $"users/{userId}/access_token", parms, this.apiKey);

            return result;
        }

        public ApiKeyResult GetApiKey(string userId)
        {
            var parms = new Dictionary<string, object>();

            var result = WebHelper.MultipartFormDataPost<ApiKeyResult>(BaseUrl + $"users/{userId}/api_key", parms, this.apiKey);

            return result;
        }


        /// <summary>
        /// Creates the organization in TrueVault.
        /// </summary>
        /// <param name="vaultName">Name of the vault in true vault. Typically will be the Org name. However, if you want to create something different, this is the field to use</param>
        /// <param name="userName">Name of the truevault user.</param>
        /// <param name="groupName">Name of the group in truevault.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Cannot create vault in TrueVault
        /// or
        /// Cannot create group in TrueVault
        /// </exception>
        public CreateOrgResult CreateOrganization(string vaultName, string userName, string groupName = "")
        {
            if (vaultName.Length > 75)
            {
                vaultName = vaultName.Substring(0, 75);
            }

            if (groupName.Length > 75)
            {
                groupName = groupName.Substring(0, 75);
            }

            //var password = EncryptionHelper.GetRandomString(20);

            //var userResult = this.CreateUser(userName, password);

            //if (userResult == null || userResult.Result != Success)
            //{
            //    throw new Exception("Cannot create user in TrueVault");
            //}

            var vaultResult = this.CreateVault(vaultName);

            if (vaultResult == null || vaultResult.Result != Success)
            {
                throw new Exception("Cannot create vault in TrueVault");
            }

            GroupFullResult groupResult = null;

            groupResult = !string.IsNullOrEmpty(groupName)
                ? this.GetGroupByName(groupName)
                : this.CreateGroup(vaultResult.Vault.Id, vaultName, string.Empty);

            if (groupResult == null && !string.IsNullOrEmpty(groupName))
            {
                groupResult = this.CreateGroup(vaultResult.Vault.Id, groupName, string.Empty);
            }

            if (groupResult == null || groupResult.Result != Success)
            {
                throw new Exception("Cannot create group in TrueVault");
            }

            return new CreateOrgResult
            {
                GroupId = groupResult.Group.Group_id,
                VaultId = vaultResult.Vault.Id
            };
        }

        public string CreateUser(List<UserOrganizationItem> organizations, string userName, string userId, AllGroupResult groups = null)
        {
            if (userName.Length > 75)
            {
                userName = userName.Substring(0, 75);
            }

            if (groups == null)
            {
                groups = this.GetAllGroups();
            }

            if (groups.Result != Success)
            {
                throw new Exception($"Cannot create user in True Vault - GroupResult: {groups.Result}");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                var password = EncryptionHelper.GetRandomString(20);

                var userResult = this.CreateUser(userName, password);

                if (userResult.Result != Success)
                {
                    throw new Exception($"Cannot create user in True Vault - UserResult: {userResult.Result}");
                }

                userId = userResult.User.Id;
            }

            this.AddUserToGroups(organizations, userId, groups);

            return userId;
        }

        public void AddUserToGroups(List<UserOrganizationItem> organizations, string userId, AllGroupResult groups, string groupId = "", string groupName = "")
        {
            foreach (var org in organizations)
            {
                var orgName = org.Organization.OrganizationName;

                if (orgName.Length > 75)
                {
                    orgName = orgName.Substring(0, 75);
                }

                var group = groups.Groups.SingleOrDefault(x => x.Group_id == org.Organization.DocumentLibraryGroupId) ??
                            groups.Groups.SingleOrDefault(x => x.Name == orgName);

                if (group == null)
                {
                    if (org.Organization.DocumentLibraryGroupId == null) continue;

                    GroupFullResult groupResult = null;

                    groupResult = this.GetGroupByName(!string.IsNullOrEmpty(groupName) ? groupName : orgName);

                    if (groupResult == null)
                    {
                        throw new Exception($"Cannot find group {orgName} in True Vault");
                    }

                    group = groupResult.Group;

                }

                groupId = group.Group_id;
                groupName = group.Name;


                try
                {
                    this.UpdateGroup(groupId, groupName, userId);
                }
                catch
                {
                    //Ignore Exception user already exists
                }

            }
        }

        public UploadResult Upload(string apiKey, string vaultId, string fileName, Stream paramFileStream)
        {
            var fileStreamContent = new StreamContent(paramFileStream);
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
                }

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent, "file", fileName);
                    var response = client.PostAsync(BaseUrl + $"vaults/{vaultId}/blobs", formData).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var str = response.Content.ReadAsStringAsync().Result;


                    return JsonConvert.DeserializeObject<UploadResult>(str);
                }
            }
        }

        public string Download(string accessToken, string vaultId, string requestValue)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
                }

                using (var formData = new MultipartFormDataContent())
                {
                    //var response = client.GetStreamAsync(BaseUrl + $"vaults/{vaultId}/blobs/{requestValue}").Result;
                    //return response;

                    var response = client.GetAsync(BaseUrl + $"vaults/{vaultId}/blobs/{requestValue}").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var str = response.Content.ReadAsStringAsync().Result;
                    return str;
                    //var test = JsonConvert.DeserializeObject<BlobResult>(str);

                    //return null;

                }
            }
        }

        public Stream DownloadFile(string accessToken, string vaultId, string requestValue)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKey);
                }

                using (var formData = new MultipartFormDataContent())
                {
                    //var response = client.GetStreamAsync(BaseUrl + $"vaults/{vaultId}/blobs/{requestValue}").Result;
                    //return response;

                    var response = client.GetAsync(BaseUrl + $"vaults/{vaultId}/blobs/{requestValue}").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var str = response.Content.ReadAsStreamAsync().Result;
                    return str;
                    //var test = JsonConvert.DeserializeObject<BlobResult>(str);

                    //return null;

                }
            }
        }
    }
}
