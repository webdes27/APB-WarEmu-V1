﻿/*
 * Copyright (C) 2011 APS
 *	http://AllPrivateServer.com
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using FrameWork;

namespace Common
{
    public class ConfigFile
    {
        private StreamReader _SR;
        private StreamWriter _SW;

        public string Name = "";
        public string Path = "";
        private string _FileName;
        private string _Path;

        public int Version = 0;
        public byte[] TotalFile;

        private Dictionary<string, BlockInfo> _Values = new Dictionary<string, BlockInfo>();
        private string _Block = "";

        public ConfigFile(string name, string path, bool load)
        {
            Log.Info(name, (load ? "Chargement+" : "") + "Ouverture : " + path);
            Name = name;
            Path = path + "/" + name;

            if (load)
                Reset(Path);
        }

        public void Reset(string path)
        {
            Log.Debug("ConfigFile", "Reset : " + path);

            try
            {
                FileStream FStream = new FileStream(path, FileMode.OpenOrCreate);

                using (_SR = new StreamReader(FStream))
                {
                    TotalFile = new byte[FStream.Length];
                    FStream.Read(TotalFile, 0, TotalFile.Length);
                    FStream.Position = 0;

                    Parse();
                }


                FStream.Close();
                _SR = null;

                if (TotalFile.Length <= 0)
                {
                    _Block = "[File]";
                    ParseLine("version=0");
                    Save();
                }
            }
            catch (Exception e)
            {
                Log.Error("FileClient", "Erreur au chargement de : " + Name + ", Erreur : " + e.ToString());
            }
        }

        public void Parse()
        {
            if (_SR == null)
                return;

            string line = "test";

            while ((line = _SR.ReadLine()) != null)
            {
                ParseLine(line);
            }

            Version = GetValue<int>("File", "version");
            Log.Debug(Name, "Version = " + Version);
        }

        public void ParseLine(string line)
        {

            if (line.Length <= 2)
                return;

            switch (line[0])
            {
                case '[': // new block
                    {
                        line = line.Remove(0, 1);
                        line = line.Remove(line.Length - 1, 1);
                        _Block = line;
                        Log.Debug("FileClient", "Nouveau Block : " + _Block);
                    } return;

                case '*': // new block
                    {
                        line = line.Remove(0, 1);
                    } break;
            }

            if (_Block == null)
                return;

            int pos = line.IndexOf('=');
            string[] result = { line.Substring(0, pos), line.Substring(pos + 1, line.Length - pos - 1) };

            if (result.Length <= 1)
                return;

            Log.Debug(_Block, result[0] + "=" + result[1]);

            if (_Values.ContainsKey(_Block))
                _Values[_Block].AddValue(result[0], result[1]);
            else
            {
                BlockInfo Info = new BlockInfo(_Block);
                Info.AddValue(result[0], result[1]);
                _Values.Add(_Block, Info);
            }
        }

        public T GetValue<T>(string Block, string name)
        {
            if (_Values.ContainsKey(Block))
                return _Values[Block].GetValue<T>(name);
            else return (T)Convert.ChangeType(0, typeof(T));
        }

        public void InitWriter()
        {
            if (_SR != null)
                _SR.Close();

            _SW = new StreamWriter(Path, false);
        }

        public void Write(byte[] Input)
        {
            Log.Debug("ConfigFile", "Ecriture dans : " + Path);
            try
            {
                FileStream FStream = new FileStream(Path, FileMode.Create);
                FStream.Write(Input, 0, Input.Length);
                FStream.Flush();
                FStream.Close();
            }
            catch (Exception e)
            {
                Log.Error("ConfigFile", "Erreur a l'écriture : " + e.ToString());
            }
            Reset(Path);
        }

        public void Save()
        {
            InitWriter();

            foreach (BlockInfo Info in _Values.Values)
                Info.SaveTo(_SW);
            _SW.Flush();
            _SW.Close();
        }

    }
}
