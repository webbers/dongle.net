using System;
using System.Collections.Generic;

namespace Dongle.System
{
    public class OsVersion
    {
        public string Name { get; set; }
        
        public string ShortName { get; set; }
        
        public string ProductType { get; set; }
        
        public string Version { get; set; }

        /// <summary>
        /// Obtém informações sobre uma versao do sistema operacional, baseada no número da versão. 
        /// A versão do Windows deve ser precedida por '2.' (ex.: 2.6.0.6000.0.0.1.256)
        /// </summary>
        /// <param name="versionValue"></param>
        public static OsVersion GetFromVersion(string versionValue)
        {
            var osVersions = versionValue.Split('.');
            var name = "Unknown";
            var shortName = "?";
            var productType = "";
            var osType = osVersions[0][0];
            var version = !versionValue.Contains("WLM") ? versionValue : versionValue.Substring(1);
            if (osVersions[0] == "2")
            {
                name = GetWindowsVersions(osVersions, name, ref shortName, ref productType);
            }
            else if (osType == 'L')
            {
                name = "Linux";
                shortName = "LINUX";
            }
            else if (osType == 'M')
            {
                name = "Mac OS";
                shortName = "MAC";
            }
            else if (osType == 'I')
            {
                name = "Ios";
                shortName = "IOS";
            }
            else if (osType == 'A')
            {
                name = "Android";
                shortName = "ADR";
            }
            var osVersion = new OsVersion
            {
                Name = name,
                ShortName = shortName,
                ProductType = productType,
                Version = version
            };
            return osVersion;
        }

        private static string GetWindowsVersions(IList<string> osVersions, string name, ref string shortName, ref string productType)
        {
            //Ex.: 2.6.0.6000.0.0.1.256
            var major = int.Parse(osVersions[1]);
            var minor = int.Parse(osVersions[2]);
            var servpack = int.Parse(osVersions[4]);
            var prodType = int.Parse(osVersions[6]);
            var suiteMask = int.Parse(osVersions[7]);
            var prodTypeServer = osVersions.Count > 8 ? Convert.ToByte(osVersions[8]) : 0;
            if (prodTypeServer == 0) prodTypeServer = prodType;

            if (major == 6)
            {
                if (minor == 0)
                {
                    if (prodTypeServer == 0x1)
                    {
                        name = "Vista";
                        shortName = "VISTA";
                    }
                    else
                    {
                        name = "Windows Server 2008";
                        shortName = "2008";
                    }
                }
                else if (minor == 2)
                {
                    if (prodTypeServer == 0x1)
                    {
                        name = "Windows 8";
                        shortName = "WIN8";
                    }
                    else
                    {
                        name = "Windows Server 2012";
                        shortName = "2012";
                    }
                }
                else
                {
                    if (prodTypeServer == 0x1)
                    {
                        name = "Windows 7";
                        shortName = "WIN7";
                    }
                    else
                    {
                        name = "Windows Server 2008 R2";
                        shortName = "2008R2";
                    }
                }
            }
            else if (major == 5 && minor == 2)
            {
                name = "Windows 2003";
                shortName = "W2003";
            }
            else if (major == 5 && minor == 1)
            {
                switch (servpack)
                {
                    case 3:
                        name = "Windows XP SP3";
                        shortName = "WXPSP3";
                        break;
                    case 2:
                        name = "Windows XP SP2";
                        shortName = "WXPSP2";
                        break;
                    default:
                        name = "Windows XP";
                        shortName = "WXP";
                        break;
                }
            }
            else if (major == 5 && minor == 0)
            {
                name = "Windows 2000";
                shortName = "W2000";
            }
            else if (major == 3)
            {
                if (minor == 0)
                {
                    name = "Windows NT 3.0";
                    shortName = "WNT30";
                }
                else if (minor == 1)
                {
                    name = "Windows NT 3.1";
                    shortName = "WNT31";
                }
                else if (minor == 51)
                {
                    name = "Windows NT 3.51";
                    shortName = "WNT35";
                }
            }
            else if (major == 4)
            {
                name = "Windows NT 4.0";
                shortName = "WNT40";
            }
            else
            {
                name = "Windows";
                shortName = "WIN";
            }
            productType = GetProductType(major, minor, prodType, suiteMask);
            name += " " + productType;
            return name;
        }

        private static string GetProductType(int major, int minor, int productType, int suiteMask)
        {
            var c = "";
            if (major == 6)
            {
                if (productType == ProductUltimate)
                {
                    c = "Ultimate";
                }
                else if (productType == ProductHomeBasic)
                {
                    c = "Home";
                }
                else if (productType == ProductHomePremium)
                {
                    c = "Premium";
                }
                else if (productType == ProductEnterprise)
                {
                    c = "Enterprise";
                }
                else if (productType == ProductHomeBasicN)
                {
                    c = "Home";
                }
                else if (productType == ProductBusiness)
                {
                    c = "Business";
                }
                else if (productType == ProductStandardServer)
                {
                    c = "Standard Server";
                }
                else if (productType == ProductDatacenterServer)
                {
                    c = "Datacenter server";
                }
                else if (productType == ProductSmallbusinessServer)
                {
                    c = "Small Business Server";
                }
                else if (productType == ProductEnterpriseServer)
                {
                    c = "Enterprise Server";
                }
                else if (productType == ProductStarter)
                {
                    c = "Starter";
                }
                else if (productType == ProductDatacenterServerCore)
                {
                    c = "Dataceter server core";
                }
                else if (productType == ProductStandardServerCore)
                {
                    c = "Standard Server Core";
                }
                else if (productType == ProductEnterpriseServerCore)
                {
                    c = "Enterprise Server Core";
                }
                else if (productType == ProductEnterpriseServerIa64)
                {
                    c = "Enterprise Server Itanium-based Systems";
                }
                else if (productType == ProductBusinessN)
                {
                    c = "Business";
                }
                else if (productType == ProductWebServer)
                {
                    c = "Web Server";
                }
                else if (productType == ProductClusterServer)
                {
                    c = "Cluster Server";
                }
                else if (productType == ProductHomeServer)
                {
                    c = "Home Server";
                }
                else if (productType == ProductStorageExpressServer)
                {
                    c = "Storage Express Server";
                }
                else if (productType == ProductStorageStandardServer)
                {
                    c = "Storage Standard Server";
                }
                else if (productType == ProductStorageWorkgroupServer)
                {
                    c = "Storage Workgroup Server";
                }
                else if (productType == ProductStorageEnterpriseServer)
                {
                    c = "Storage Enterprise Server";
                }
                else if (productType == ProductServerForSmallbusiness)
                {
                    c = "Small Business Server";
                }
                else if (productType == ProductSmallbusinessServerPremium)
                {
                    c = "Small Business Server Premium";
                }
                else if (productType == ProductHomePremiumN)
                {
                    c = "Home Premium";
                }
                else if (productType == ProductEnterpriseN)
                {
                    c = "Enterprise";
                }
                else if (productType == ProductUltimateN)
                {
                    c = "Ultimate";
                }
                else if (productType == ProductWebServerCore)
                {
                    c = "Web Server Core";
                }
                else if (productType == ProductMediumbusinessServerManagement)
                {
                    c = "Medium Business Server Management";
                }
                else if (productType == ProductMediumbusinessServerSecurity)
                {
                    c = "Medium Business Server Security";
                }
                else if (productType == ProductMediumbusinessServerMessaging)
                {
                    c = "Medium Business Server Messaging";
                }
                else if (productType == ProductSmallbusinessServerPrime)
                {
                    c = "Small Business Server Prime";
                }
                else if (productType == ProductHomePremiumServer)
                {
                    c = "Home Premium Server";
                }
                else if (productType == ProductServerForSmallbusinessV)
                {
                    c = "Small Business Server";
                }
                else if (productType == ProductStandardServerV)
                {
                    c = "Standard Server";
                }
                else if (productType == ProductDatacenterServerV)
                {
                    c = "Datacenter Server";
                }
                else if (productType == ProductEnterpriseServerV)
                {
                    c = "Datacenter Server";
                }
                else if (productType == ProductDatacenterServerCoreV)
                {
                    c = "Datacenter Server Core";
                }
                else if (productType == ProductStandardServerCoreV)
                {
                    c = "Standard Server Core";
                }
                else if (productType == ProductEnterpriseServerCoreV)
                {
                    c = "Enterprise Server Core";
                }
                else if (productType == ProductHyperv)
                {
                    c = "Hyperv";
                }
                else if (productType == ProductStorageExpressServerCore)
                {
                    c = "Storege Express Server Core";
                }
                else if (productType == ProductStorageStandardServerCore)
                {
                    c = "Storege Standard Server Core";
                }
                else if (productType == ProductStorageWorkgroupServerCore)
                {
                    c = "Storege Workgroup Server Core";
                }
                else if (productType == ProductStorageEnterpriseServerCore)
                {
                    c = "Storege Enterprise Server Core";
                }
                else if (productType == ProductProfessional)
                {
                    c = "Professional";
                }
                else if (productType == ProductProfessionalN)
                {
                    c = "Professional";
                }
                else if (productType == ProductSbSolutionServer)
                {
                    c = "Solution Server";
                }
                else if (productType == ProductServerForSbSolutions)
                {
                    c = "Solution Server";
                }
                else if (productType == ProductStandardServerSolutions)
                {
                    c = "Standard Solution Server";
                }
                else if (productType == ProductStandardServerSolutionsCore)
                {
                    c = "Standard Solution Server Core";
                }
                else if (productType == ProductSbSolutionServerEm)
                {
                    c = "Solution Server";
                }
                else if (productType == ProductServerForSbSolutionsEm)
                {
                    c = "Solution Server";
                }
                else if (productType == ProductSolutionEmbeddedserver)
                {
                    c = "Solution Embedded Server";
                }
                else if (productType == ProductSolutionEmbeddedserverCore)
                {
                    c = "Solution Embedded Server Core";
                }
                else if (productType == ProductSmallbusinessServerPremiumCore)
                {
                    c = "Small Business Server Premium Core";
                }
                else if (productType == ProductEssentialbusinessServerMgmt)
                {
                    c = "Essential Business Server";
                }
                else if (productType == ProductEssentialbusinessServerAddl)
                {
                    c = "Essential Business Server";
                }
                else if (productType == ProductEssentialbusinessServerMgmtsvc)
                {
                    c = "Essential Business Server";
                }
                else if (productType == ProductEssentialbusinessServerAddlsvc)
                {
                    c = "Essential Business Server";
                }
                else if (productType == ProductClusterServerV)
                {
                    c = "Cluster Server";
                }
                else if (productType == ProductEmbedded)
                {
                    c = "Embedded";
                }
                else if (productType == ProductStarterE)
                {
                    c = "Starter";
                }
                else if (productType == ProductHomeBasicE)
                {
                    c = "Home";
                }
                else if (productType == ProductHomePremiumE)
                {
                    c = "Premium";
                }
                else if (productType == ProductProfessionalE)
                {
                    c = "Professional";
                }
                else if (productType == ProductEnterpriseE)
                {
                    c = "Enterprise";
                }
                else if (productType == ProductUltimateE)
                {
                    c = "Ultimate";
                }
            }
            else
            {
                if (productType == ProductTypeWorkstation)
                {
                    if (major < 5)
                        c = "Workstation";
                    else if ((suiteMask & ProductSuitePersonal) == ProductSuitePersonal)
                        c = "Home Edition";
                    else
                        c = "Professional";
                }
                else if (productType == ProductTypeDomainController)
                {
                    c = "Domain Controller Server";
                }
                else if (productType == ProductTypeServer)
                {
                    if ((suiteMask & ProductSuiteDataCenter) == ProductSuiteDataCenter)
                        c = "Datacenter Server";
                    else if ((suiteMask & ProductSuiteEnterprise) == ProductSuiteEnterprise)
                    {
                        if (major == 5 && minor == 0)
                            c = "Advanced Server";
                        else
                            c = "Edition Entreprise";
                    }
                    else if ((suiteMask & ProductSuiteBlade) == ProductSuiteBlade)
                    {
                        c = "Edition Web";
                    }
                    else
                    {
                        c = "Server";
                    }
                }
            }
            return c;
        }

        private const int ProductUltimate = 0x1;
        private const int ProductHomeBasic = 0x2;
        private const int ProductHomePremium = 0x3;
        private const int ProductEnterprise = 0x4;
        private const int ProductHomeBasicN = 0x5;
        private const int ProductBusiness = 0x6;
        private const int ProductStandardServer = 0x7;
        private const int ProductDatacenterServer = 0x8;
        private const int ProductSmallbusinessServer = 0x9;
        private const int ProductEnterpriseServer = 0xA;
        private const int ProductStarter = 0xB;
        private const int ProductDatacenterServerCore = 0xC;
        private const int ProductStandardServerCore = 0xD;
        private const int ProductEnterpriseServerCore = 0xE;
        private const int ProductEnterpriseServerIa64 = 0xF;
        private const int ProductBusinessN = 0x10;
        private const int ProductWebServer = 0x11;
        private const int ProductClusterServer = 0x12;
        private const int ProductHomeServer = 0x13;
        private const int ProductStorageExpressServer = 0x14;
        private const int ProductStorageStandardServer = 0x15;
        private const int ProductStorageWorkgroupServer = 0x16;
        private const int ProductStorageEnterpriseServer = 0x17;
        private const int ProductServerForSmallbusiness = 0x18;
        private const int ProductSmallbusinessServerPremium = 0x19;
        private const int ProductHomePremiumN = 0x1A;
        private const int ProductEnterpriseN = 0x1B;
        private const int ProductUltimateN = 0x1C;
        private const int ProductWebServerCore = 0x1D;
        private const int ProductMediumbusinessServerManagement = 0x1E;
        private const int ProductMediumbusinessServerSecurity = 0x1F;
        private const int ProductMediumbusinessServerMessaging = 0x20;
        private const int ProductSmallbusinessServerPrime = 0x21;
        private const int ProductHomePremiumServer = 0x22;
        private const int ProductServerForSmallbusinessV = 0x23;
        private const int ProductStandardServerV = 0x24;
        private const int ProductDatacenterServerV = 0x25;
        private const int ProductEnterpriseServerV = 0x26;
        private const int ProductDatacenterServerCoreV = 0x27;
        private const int ProductStandardServerCoreV = 0x28;
        private const int ProductEnterpriseServerCoreV = 0x29;
        private const int ProductHyperv = 0x2A;
        private const int ProductStorageExpressServerCore = 0x2B;
        private const int ProductStorageStandardServerCore = 0x2C;
        private const int ProductStorageWorkgroupServerCore = 0x2D;
        private const int ProductStorageEnterpriseServerCore = 0x2E;
        private const int ProductProfessional = 0x30;
        private const int ProductProfessionalN = 0x31;
        private const int ProductSbSolutionServer = 0x32;
        private const int ProductServerForSbSolutions = 0x33;
        private const int ProductStandardServerSolutions = 0x34;
        private const int ProductStandardServerSolutionsCore = 0x35;
        private const int ProductSbSolutionServerEm = 0x36;
        private const int ProductServerForSbSolutionsEm = 0x37;
        private const int ProductSolutionEmbeddedserver = 0x38;
        private const int ProductSolutionEmbeddedserverCore = 0x39;
        private const int ProductSmallbusinessServerPremiumCore = 0x3F;
        private const int ProductEssentialbusinessServerMgmt = 0x3B;
        private const int ProductEssentialbusinessServerAddl = 0x3C;
        private const int ProductEssentialbusinessServerMgmtsvc = 0x3D;
        private const int ProductEssentialbusinessServerAddlsvc = 0x3E;
        private const int ProductClusterServerV = 0x40;
        private const int ProductEmbedded = 0x41;
        private const int ProductStarterE = 0x42;
        private const int ProductHomeBasicE = 0x43;
        private const int ProductHomePremiumE = 0x44;
        private const int ProductProfessionalE = 0x45;
        private const int ProductEnterpriseE = 0x46;
        private const int ProductUltimateE = 0x47;
        private const int ProductSuiteEnterprise = 0x2;
        private const int ProductSuiteDataCenter = 0x80;
        private const int ProductSuitePersonal = 0x200;
        private const int ProductSuiteBlade = 0x400;
        private const int ProductTypeWorkstation = 0x1;
        private const int ProductTypeDomainController = 0x2;
        private const int ProductTypeServer = 0x3;
    }
}
