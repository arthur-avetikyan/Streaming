using System;
using System.IO;
using System.Threading.Tasks;
using KioskStream.Models;
using KioskStream.Web.Common.DataTransferObjects.Account;

using KioskStream.Data.Models;
using KioskStream.Web.Common.DataTransferObjects.RequestResponse;
using Microsoft.AspNetCore.Http;
using MatBlazor;

namespace KioskStream.Web.Common.Utils
{
    public static class MappingExtensions
    {
        public static UserDetails Map(this User entity)
        {
            return new UserDetails
            {
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public static User Map(this RegisterParameters dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email
            };
        }

        public static Kiosk Map(this KioskRequest dto, Kiosk kiosk)
        {
            kiosk.Name = dto.Name;
            kiosk.Approved = dto.Approved;
            kiosk.KioskIdentifier = dto.KioskIdentifier;
            kiosk.Location = dto.Location;
            kiosk.TimeZone = dto.TimeZone;

            return kiosk;
        }

        public static KioskResponse Map(this Kiosk dto)
        {
            return new KioskResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Approved = dto.Approved,
                KioskIdentifier = dto.KioskIdentifier,
                Location = dto.Location,
                TimeZone = dto.TimeZone,
                CreateDateTimeUtc = dto.CreateDateTimeUtc
            };
        }


        public static async Task<Plugin> Map(this PluginRequest dto, Plugin entity)
        {
            entity.Name = dto.Name;
            entity.Path = await SaveFile(dto, "plugins");

            return entity;
        }

        public static PluginResponse Map(this Plugin dto)
        {
            return new PluginResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                CreateDateTimeUtc = dto.CreateDateTimeUtc,
                Path = dto.Path,
            };
        }

        public static async Task<string> SaveFile(PluginRequest dto, string rootFolder)
        {

            if (dto == null || dto.Plugin == null || dto.Name == null)
            {
                return string.Empty;
            }

            string pluginName = Path.GetFileNameWithoutExtension(dto.FileName)?.ToLower().Replace(" ", "_");
            string extension = Path.GetExtension(dto.FileName);
            string imagesPath = CreateDirectoryIfNot(rootFolder);
            string imagePathToSave = imagesPath + "/" + pluginName + extension;
            try
            {
                await File.WriteAllBytesAsync(imagePathToSave, dto.Plugin);
                return rootFolder + "/" + pluginName + extension;

            }
            catch (Exception e)
            {
                if (!e.Message.Contains("exists"))
                {
                    Console.WriteLine(e);
                }
                //TODO maybe need to handle
            }

            return rootFolder + "/" + pluginName + extension;
        }


        public static string CreateDirectoryIfNot(string folderPathFromRoot)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fullPathToDir = rootFolder + "/" + folderPathFromRoot;
            if (Directory.Exists(fullPathToDir))
            {
                return fullPathToDir;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(fullPathToDir);
                    return fullPathToDir;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                    return "";
                }
            }
        }
    }
}
