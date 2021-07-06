// <copyright file="GlobalSuppressions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.GameRepository.LoadSaved(TankPocalypse.Repository.Interfaces.ISavedGame)")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.GameRepository.UpdateProfile(System.Int32,System.String,System.Boolean,System.Int32,System.Int32)")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Interfaces.IGameRepository.GetAllMaps~System.Collections.Generic.List{TankPocalypse.Repository.Interfaces.IMap}")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Interfaces.IGameRepository.GetAllProfiles~System.Collections.Generic.List{TankPocalypse.Repository.Interfaces.IProfile}")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Interfaces.IGameRepository.GetSavedGames~System.Collections.Generic.List{TankPocalypse.Repository.Interfaces.ISavedGame}")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Classes.Map.#ctor(System.String,System.Collections.Generic.List{System.String})")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Classes.Profile.CreateFromXml(System.Xml.Linq.XDocument)~TankPocalypse.Repository.Interfaces.IProfile")]
[assembly: SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<NotRequired>", Scope = "member", Target = "~P:TankPocalypse.Repository.Interfaces.IMap.MapData")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<NotRequired>", Scope = "member", Target = "~M:TankPocalypse.Repository.Classes.RepoProfile.CreateFromXml(System.Xml.Linq.XDocument)~TankPocalypse.Repository.Interfaces.IProfile")]
