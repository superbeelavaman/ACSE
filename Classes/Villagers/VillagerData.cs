﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using ACSE.Classes.Utilities;

namespace ACSE
{
    internal class VillagerData
    {
        public static readonly Dictionary<ushort, string> WA_Special_Villagers = new Dictionary<ushort, string>
        {
            {0x1000, "Copper"},
            {0x1001, "Booker"},
            {0x1002, "Jingle"},
            {0x1003, "Jack"},
            {0x1004, "Zipper"},
            {0x1005, "Blanca"},
            {0x1006, "Pavé"},
            {0x1007, "Chip"},
            {0x1008, "Nat"},
            {0x1009, "Franklin"},
            {0x100A, "Joan"},
            {0x100B, "Wendell"},
            {0x100C, "Pascal"},
            {0x100D, "Gulliver"},
            {0x100E, "Saharah"},
            {0x2000, "Isabelle (1)"},
            {0x2001, "Digby"},
            {0x2002, "Reese"},
            {0x2003, "Cyrus"},
            {0x2004, "Tom Nook"},
            {0x2005, "Lottie"},
            {0x2006, "Mabel"},
            {0x2007, "K.K. Slider"},
            {0x2008, "Kicks"},
            {0x2009, "Resetti"},
            {0x200A, "Celeste"},
            {0x200B, "Blathers"},
            {0x200C, "Rover"},
            {0x200D, "Timmy"},
            {0x200E, "Kapp'n"},
            {0x200F, "Wisp"},
            {0x2010, "Isabelle (2)"}
        };

        public static BindingSource GetCaravanBindingSource()
        {
            var waDatabase = VillagerInfo.GetVillagerDatabase(SaveType.Welcome_Amiibo);
            if (waDatabase == null) return new BindingSource(WA_Special_Villagers, null);
            foreach (var v in WA_Special_Villagers)
            {
                var specialVillager = new SimpleVillager
                {
                    VillagerId = v.Key,
                    Name = v.Value
                };
                waDatabase.Add(v.Key, specialVillager);
            }

            return new BindingSource(waDatabase, null);
        }
    }

    public struct VillagerOffsets
    {
        public int VillagerId;
        public int NameId;
        public int Catchphrase;
        public int CatchphraseSize;
        public int Nicknames;
        public int Personality;
        public int TownId;
        public int TownName;
        public int TownNameSize;
        public int Shirt;
        public int Umbrella;
        public int Song;
        public int Carpet;
        public int Wallpaper;
        public int Furniture;
        public int FurnitureCount;
        public int HouseCoordinates;
        public int HouseCoordinatesCount;
        public int Status;
    }

    //Rename when VillagerData class is removed
    public struct VillagerDataStruct
    {
        public ushort VillagerId;
        public byte NameId; // In N64/GC, this is their "speech pattern" and not their "AI"
        public string Catchphrase;
        public byte Personality;
        public ushort TownId;
        public string TownName;
        public Item Shirt;
        public Item Umbrella;
        public Item Song;
        public Item Carpet;
        public Item Wallpaper;
        public Item[] Furniture;

        public byte[]
            HouseCoordinates; //(In N64 & GCN, it's X-Acre, Y-Acre, X-Position, Y-Position)[X/Y Positions are the bottom center item (house item + 0x10)]

        public byte Status;
        //Player Entries?
    }

    public static class VillagerInfo
    {
        public static readonly VillagerOffsets Doubutsu_no_Mori_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            TownId = 2,
            TownName = 4,
            TownNameSize = 6,
            NameId = 0xA,
            Personality = 0xB,
            HouseCoordinates = 0x4E1,
            HouseCoordinatesCount = 4,
            Catchphrase = 0x4E5,
            CatchphraseSize = 0x4,
            Shirt = 0x520
        };

        public static readonly VillagerOffsets Doubtusu_no_Mori_Plus_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            TownId = 2,
            TownName = 4,
            TownNameSize = 6,
            NameId = 0xA, // Goes unused??
            Personality = 0xB,
            HouseCoordinates = 0x4E1,
            HouseCoordinatesCount = 4,
            Catchphrase = 0x4E5,
            CatchphraseSize = 0x4,
            Shirt = 0x520,
            Status = -1, //Research
            Umbrella = -1, //Research this as well
            Furniture = -1, //No Furniture customization in AC
            Carpet = -1,
            Wallpaper = -1,
            Nicknames = -1, //Inside of "Player Entries"
            Song = -1,
            //Add Player Entries (Relationships)
        };

        public static readonly VillagerOffsets AC_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            TownId = 2,
            TownName = 4,
            TownNameSize = 8,
            NameId = 0xC,
            Personality = 0xD,
            HouseCoordinates = 0x899,
            HouseCoordinatesCount = 4,
            Catchphrase = 0x89D,
            CatchphraseSize = 0xA,
            Shirt = 0x8E4,
            Status = -1, //Research
            Umbrella = -1, //Research this as well
            Furniture = -1, //No Furniture customization in AC
            Carpet = -1,
            Wallpaper = -1,
            Nicknames = -1, //Inside of "Player Entries"
            Song = -1,
            //Add Player Entries (Relationships)
        };

        public static readonly VillagerOffsets Doubtusu_no_Mori_e_Plus_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            TownId = 2,
            TownName = 4,
            TownNameSize = 6,
            NameId = 0xA, // Goes unused??
            Personality = 0xB,
            HouseCoordinates = 0x591, // Confirm
            HouseCoordinatesCount = 4,
            Catchphrase = 0x595, // Confirm
            CatchphraseSize = 0x4,
            Shirt = 0x5DA,
            Status = -1, //Research
            Umbrella = -1, //Research this as well
            Furniture = -1, //No Furniture customization in AC
            Carpet = -1,
            Wallpaper = -1,
            Nicknames = -1, //Inside of "Player Entries"
            Song = -1,
            //Add Player Entries (Relationships)
        };

        public static readonly VillagerOffsets WW_Villager_Offsets = new VillagerOffsets
        {
            //0 = Relationships (0x68 bytes each)
            //Pattern as well
            Furniture = 0x6AC,
            FurnitureCount = 0xA,
            Personality = 0x6CA,
            VillagerId = 0x6CB,
            Wallpaper = 0x6CC,
            Carpet = 0x6CE,
            Song = 0x6D0, //Check this
            Shirt = 0x6EC,
            Catchphrase = 0x6DE,
            CatchphraseSize = 0xA,
            NameId = -1, // Research
            TownId = -1, //Research
            TownName = -1, //Research
            HouseCoordinates = -1, //Research
            Nicknames = -1, //Research
            Status = -1, //Research
            Umbrella = -1, //Research
            //Finish rest of offsets
        };

        public static readonly VillagerOffsets CF_Villager_Offsets = new VillagerOffsets
        {
            //Villagers in City Folk are interesting.
            //The actual data structure is stored in the save file, allowing for customization of the entire villager.
            //This includes name, textures, and what villager model it uses!
            //That will mean a lot more work will have to go into this part of the villager editor, though.
            //I'll have to finish it at a later date. Unfortunately, I can't find the source to NPC Tool, a tool that allowed all of these modifications to be done
            //This means I'll probably have to reverse engineer the format myself
        };

        public static readonly VillagerOffsets NL_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            Personality = 2,
            Status = 0x24C4,
            Catchphrase = 0x24A6,
            CatchphraseSize = 0x16,
            TownName = 0x24CE,
            TownNameSize = 0x12,
            TownId = -1, // Research
            Shirt = 0x244E,
            Song = 0x2452,
            Wallpaper = 0x2456,
            Carpet = 0x245A,
            Umbrella = 0x245E,
            Furniture = 0x2462,
            FurnitureCount = 16,
            HouseCoordinates = -1,
            Nicknames = -1, //Research
            NameId = -1,
        };

        public static readonly VillagerOffsets WA_Villager_Offsets = new VillagerOffsets
        {
            VillagerId = 0,
            Personality = 2,
            Status = 0x24E4,
            Catchphrase = 0x24C6,
            CatchphraseSize = 0x16,
            TownName = 0x24EE,
            TownNameSize = 0x12,
            TownId = -1, // Research
            Shirt = 0x246E,
            Song = 0x2472,
            Wallpaper = 0x2476,
            Carpet = 0x247A,
            Umbrella = 0x247E,
            Furniture = 0x2482,
            FurnitureCount = 16,
            HouseCoordinates = -1,
            Nicknames = -1, //Research
            NameId = -1,
        };

        public static readonly string[] AC_Personalities =
        {
            "Normal ♀", "Peppy ♀", "Lazy ♂", "Jock ♂", "Cranky ♂", "Snooty ♀", "Not Set"
        };

        public static readonly string[] WW_Personalities =
        {
            "Lazy ♂", "Jock ♂", "Cranky ♂", "Normal ♀", "Peppy ♀", "Snooty ♀", "Not Set"
        };

        public static readonly string[] NL_Personalities =
        {
            "Lazy ♂", "Jock ♂", "Cranky ♂", "Smug ♂", "Normal ♀", "Peppy ♀", "Snooty ♀", "Uchi ♀", "Not Set"
        };

        public static string[] GetPersonalities(SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.Doubutsu_no_Mori:
                case SaveType.Doubutsu_no_Mori_Plus:
                case SaveType.Animal_Crossing:
                case SaveType.Doubutsu_no_Mori_e_Plus:
                case SaveType.Animal_Forest:
                    return AC_Personalities;
                case SaveType.Wild_World:
                    return WW_Personalities;
                case SaveType.New_Leaf:
                    return NL_Personalities;
                case SaveType.Welcome_Amiibo:
                    return NL_Personalities;
                case SaveType.Unknown:
                    break;
                case SaveType.City_Folk:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
            }

            return new string[0];
        }

        public static Dictionary<ushort, SimpleVillager> GetVillagerDatabase(SaveType saveType, string language = "en")
        {
            var database = new Dictionary<ushort, SimpleVillager>();
            StreamReader contents;
            var databaseFilename = MainForm.Assembly_Location + "\\Resources\\{0}_Villagers_" + language + ".txt";
            switch (saveType)
            {
                case SaveType.Doubutsu_no_Mori:
                case SaveType.Animal_Forest:
                    databaseFilename = string.Format(databaseFilename, "DnM");
                    break;
                case SaveType.Doubutsu_no_Mori_Plus:
                case SaveType.Doubutsu_no_Mori_e_Plus:
                case SaveType.Animal_Crossing:
                    databaseFilename = string.Format(databaseFilename, "AC");
                    break;
                case SaveType.Wild_World:
                    databaseFilename = string.Format(databaseFilename, "WW");
                    break;
                case SaveType.New_Leaf:
                    databaseFilename = string.Format(databaseFilename, "NL");
                    break;
                case SaveType.Welcome_Amiibo:
                    databaseFilename = string.Format(databaseFilename, "WA");
                    break;
                case SaveType.Unknown:
                    break;
                case SaveType.City_Folk:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
            }

            try
            {
                contents = File.OpenText(databaseFilename);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"An error occured opening villager database file:\n\"{databaseFilename}\"\nError Info:\n{e.Message}");
                return null;
            }

            string line;
            switch (saveType)
            {
                case SaveType.New_Leaf:
                case SaveType.Welcome_Amiibo:
                    while ((line = contents.ReadLine()) != null)
                    {
                        if (!line.Contains("0x")) continue;
                        var entry = new SimpleVillager();
                        var id = Regex.Match(line, @"ID = 0x....,").Value.Substring(7, 4);
                        entry.VillagerId = ushort.Parse(id, NumberStyles.AllowHexSpecifier);
                        var nameStr = Regex.Match(line, @"Name = .+").Value.Substring(7);
                        entry.Name = nameStr.Substring(0, nameStr.IndexOf(','));
                        var personality = Regex.Match(line, @"Personality = .").Value;
                        entry.Personality = byte.Parse(personality.Substring(personality.Length - 1, 1));
                        database.Add(entry.VillagerId, entry);
                    }

                    break;
                case SaveType.Wild_World:
                    while ((line = contents.ReadLine()) != null)
                    {
                        if (!line.Contains("0x")) continue;
                        var entry = new SimpleVillager
                        {
                            VillagerId = ushort.Parse(line.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                            Name = line.Substring(6)
                        };
                        database.Add(entry.VillagerId, entry);
                    }

                    break;
                case SaveType.Doubutsu_no_Mori:
                case SaveType.Animal_Crossing:
                case SaveType.Doubutsu_no_Mori_Plus:
                case SaveType.Doubutsu_no_Mori_e_Plus:
                case SaveType.Animal_Forest:
                    while ((line = contents.ReadLine()) != null)
                    {
                        if (!line.Contains("0x")) continue;
                        var entry = new SimpleVillager
                        {
                            VillagerId = ushort.Parse(line.Substring(2, 4), NumberStyles.AllowHexSpecifier),
                            Name = line.Substring(8)
                        };
                        database.Add(entry.VillagerId, entry);
                    }

                    break;
                case SaveType.Unknown:
                    break;
                case SaveType.City_Folk:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
            }

            contents.Close();
            contents.Dispose();

            return database;
        }

        public static VillagerOffsets GetVillagerInfo(SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.Doubutsu_no_Mori:
                    return Doubutsu_no_Mori_Villager_Offsets;
                case SaveType.Doubutsu_no_Mori_Plus:
                    return Doubtusu_no_Mori_Plus_Villager_Offsets;
                case SaveType.Animal_Crossing:
                    return AC_Villager_Offsets;
                case SaveType.Doubutsu_no_Mori_e_Plus:
                    return Doubtusu_no_Mori_e_Plus_Villager_Offsets;
                case SaveType.Animal_Forest:
                    return Doubutsu_no_Mori_Villager_Offsets; // TEMP
                case SaveType.Wild_World:
                    return WW_Villager_Offsets;
                case SaveType.New_Leaf:
                    return NL_Villager_Offsets;
                case SaveType.Welcome_Amiibo:
                    return WA_Villager_Offsets;
                case SaveType.Unknown:
                    break;
                case SaveType.City_Folk:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
            }

            return new VillagerOffsets();
        }
    }
}
