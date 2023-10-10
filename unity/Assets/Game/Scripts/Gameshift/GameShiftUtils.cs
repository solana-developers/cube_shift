using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SimpleJSON
{
    /*
        Test user: testUser
        Collection
        {
          "id": "eaf23fb5-7030-4cea-b065-5615597993df",
          "name": "Cube Shift",
          "imageUrl": "https://crossmint.myfilebase.com/ipfs/bafkreif22zhtjvdmke4ddguinzqezklpsqsyzonqedhvsxdy5zbcjiddke",
          "sourceImageUrl": "https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/CubeCollectionImage.png"
        }
        
        Firemage URL: https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/FireMage.png
        
        Monkey URL: https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/Monkey.png
    */

    [Serializable]
    public class AssetsResultJson
    {
        public AssetsResult json;
    }

    [Serializable]
    public class AssetsResult
    {
        public List<Attribute> data;
        public AssetMetaData meta;
    }

    [Serializable]
    public class AssetMetaData
    {
        public int page;
        public int perPage;
        public int totalPages;
    }
    
    [Serializable]
    public class Attribute
    {
        public string id;
        public string collectionId;
        public List<AttributeDat> attributes;
        public string name;
        public string description;
        public string imageUrl;
        public string status;
        public List<OwnerData> owner;
    }

    [Serializable]
    public class OwnerData
    {
        public string referenceId;
    }
    
    [Serializable]
    public class AttributeDat
    {
        public string value;
        public string traitType;
    }

    public class GameShiftUtils
    {
        public static string API_KEY = "ADD_YOUR_API_KEY";
        
        public static string CreateRandomId()
        {
            const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
            string myString = "";
            for(int i=0; i<18; i++)
            {
                myString += glyphs[Random.Range(0, glyphs.Length)];
            }

            return myString;
        }
        
        public static string GenerateFireMageJson(string destinationUserReferenceId)
        {
            string jsonString = @"
            {
                ""details"": {
                    ""attributes"": [
                        {
                            ""traitType"": ""FireBall"",
                            ""value"": ""1""
                        },
                        {
                            ""traitType"": ""FireBolt"",
                            ""value"": ""2""
                        }
                    ],
                    ""collectionId"": ""eaf23fb5-7030-4cea-b065-5615597993df"",
                    ""description"": ""Starts with 3 extra fire Spells."",
                    ""imageUrl"": ""https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/FireMage.png"",
                    ""name"": ""Fire Mage""
                },
                ""destinationUserReferenceId"": """ + destinationUserReferenceId + @"""
            }";

            return jsonString;
        }
        
        public static string GenerateMonkeyJson(string destinationUserReferenceId)
        {
            string jsonString = @"
                {
                    ""details"": {
                    ""attributes"": [
                        {
                            ""traitType"": ""Banana"",
                            ""value"": ""3""
                        }
                    ],
                    ""collectionId"": ""eaf23fb5-7030-4cea-b065-5615597993df"",
                    ""description"": ""Starts with 3 extra rotating Bananas."",
                    ""imageUrl"": ""https://shdw-drive.genesysgo.net/AzjHvXgqUJortnr5fXDG2aPkp2PfFMvu4Egr57fdiite/Monkey.png"",
                    ""name"": ""Monkey""
                },
                ""destinationUserReferenceId"": """ + destinationUserReferenceId + @"""
            }";

            return jsonString;
        }
    }
}