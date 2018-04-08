SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

CREATE DATABASE IF NOT EXISTS `the_tennis_project` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `the_tennis_project`;

CREATE TABLE `backup_matches` (
  `ID` bigint(20) UNSIGNED NOT NULL,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_year` int(11) DEFAULT NULL,
  `tourney_id` varchar(5) COLLATE utf8_bin NOT NULL,
  `tourney_name` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_level_id` tinyint(3) UNSIGNED NOT NULL,
  `tourney_date` datetime NOT NULL,
  `surface_id` tinyint(3) UNSIGNED DEFAULT NULL,
  `draw_size` smallint(6) NOT NULL,
  `match_num` smallint(5) UNSIGNED DEFAULT NULL,
  `winner_ID` bigint(20) UNSIGNED NOT NULL,
  `winner_seed` int(10) UNSIGNED DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) UNSIGNED DEFAULT NULL,
  `winner_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `loser_ID` bigint(20) UNSIGNED NOT NULL,
  `loser_seed` int(10) UNSIGNED DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) UNSIGNED DEFAULT NULL,
  `loser_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `round` varchar(5) COLLATE utf8_bin NOT NULL,
  `best_of` tinyint(3) UNSIGNED NOT NULL,
  `minutes` int(10) UNSIGNED DEFAULT NULL,
  `unfinished` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `retirement` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `walkover` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `w_ace` int(10) UNSIGNED DEFAULT NULL,
  `w_df` int(10) UNSIGNED DEFAULT NULL,
  `w_svpt` int(10) UNSIGNED DEFAULT NULL,
  `w_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `w_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `w_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `w_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `w_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `w_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `l_ace` int(10) UNSIGNED DEFAULT NULL,
  `l_df` int(10) UNSIGNED DEFAULT NULL,
  `l_svpt` int(10) UNSIGNED DEFAULT NULL,
  `l_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `l_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `l_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `l_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `l_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `l_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `w_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `tb_set_1` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_2` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_3` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_4` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_5` smallint(5) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_matches_davis` (
  `ID` bigint(20) UNSIGNED NOT NULL,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_year` int(11) DEFAULT NULL,
  `tourney_id` varchar(5) COLLATE utf8_bin NOT NULL,
  `tourney_name` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_level_id` tinyint(3) UNSIGNED NOT NULL,
  `tourney_date` datetime NOT NULL,
  `surface_id` tinyint(3) UNSIGNED DEFAULT NULL,
  `draw_size` smallint(6) NOT NULL,
  `match_num` smallint(5) UNSIGNED DEFAULT NULL,
  `winner_ID` bigint(20) UNSIGNED NOT NULL,
  `winner_seed` int(10) UNSIGNED DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) UNSIGNED DEFAULT NULL,
  `winner_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `loser_ID` bigint(20) UNSIGNED NOT NULL,
  `loser_seed` int(10) UNSIGNED DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) UNSIGNED DEFAULT NULL,
  `loser_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `round` varchar(5) COLLATE utf8_bin NOT NULL,
  `best_of` tinyint(3) UNSIGNED NOT NULL,
  `minutes` int(10) UNSIGNED DEFAULT NULL,
  `unfinished` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `retirement` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `walkover` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `w_ace` int(10) UNSIGNED DEFAULT NULL,
  `w_df` int(10) UNSIGNED DEFAULT NULL,
  `w_svpt` int(10) UNSIGNED DEFAULT NULL,
  `w_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `w_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `w_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `w_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `w_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `w_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `l_ace` int(10) UNSIGNED DEFAULT NULL,
  `l_df` int(10) UNSIGNED DEFAULT NULL,
  `l_svpt` int(10) UNSIGNED DEFAULT NULL,
  `l_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `l_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `l_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `l_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `l_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `l_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `w_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `tb_set_1` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_2` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_3` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_4` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_5` smallint(5) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_tourneys_indoor_history` (
  `ID` int(10) UNSIGNED NOT NULL,
  `is_indoor` tinyint(1) UNSIGNED NOT NULL,
  `last_year` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_tourneys_level_history` (
  `ID` int(10) UNSIGNED NOT NULL,
  `level_ID` tinyint(3) UNSIGNED NOT NULL,
  `last_year` int(10) UNSIGNED NOT NULL,
  `substitute_ID` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_tourneys_name_and_city_history` (
  `ID` int(10) UNSIGNED NOT NULL,
  `name` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `city` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `last_year` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_tourneys_slot_history` (
  `ID` int(10) UNSIGNED NOT NULL,
  `slot_order` tinyint(3) UNSIGNED NOT NULL,
  `last_year` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `backup_tourneys_surface_history` (
  `ID` int(10) UNSIGNED NOT NULL,
  `surface_ID` tinyint(3) UNSIGNED NOT NULL,
  `last_year` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `editions` (
  `ID` int(10) UNSIGNED NOT NULL,
  `tournament_ID` int(10) UNSIGNED NOT NULL,
  `year` int(10) UNSIGNED NOT NULL,
  `draw_size` smallint(5) UNSIGNED NOT NULL,
  `date_begin` datetime NOT NULL,
  `surface_ID` tinyint(3) UNSIGNED NOT NULL,
  `slot_order` tinyint(3) UNSIGNED DEFAULT NULL,
  `is_indoor` tinyint(1) UNSIGNED NOT NULL,
  `level_ID` tinyint(3) UNSIGNED NOT NULL,
  `substitute_ID` int(10) UNSIGNED DEFAULT NULL,
  `name` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `city` varchar(255) COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `levels` (
  `ID` tinyint(3) UNSIGNED NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `matches` (
  `ID` bigint(20) UNSIGNED NOT NULL,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `edition_ID` int(10) UNSIGNED NOT NULL,
  `match_num` smallint(5) UNSIGNED NOT NULL,
  `round_ID` tinyint(3) UNSIGNED NOT NULL,
  `best_of` tinyint(3) UNSIGNED NOT NULL,
  `winner_ID` bigint(20) UNSIGNED NOT NULL,
  `winner_seed` int(10) UNSIGNED DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) UNSIGNED DEFAULT NULL,
  `winner_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `loser_ID` bigint(20) UNSIGNED NOT NULL,
  `loser_seed` int(10) UNSIGNED DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) UNSIGNED DEFAULT NULL,
  `loser_rank_points` int(10) UNSIGNED DEFAULT NULL,
  `minutes` int(10) UNSIGNED DEFAULT NULL,
  `unfinished` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `retirement` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `walkover` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `w_ace` int(10) UNSIGNED DEFAULT NULL,
  `w_df` int(10) UNSIGNED DEFAULT NULL,
  `w_svpt` int(10) UNSIGNED DEFAULT NULL,
  `w_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `w_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `w_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `w_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `w_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `w_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `l_ace` int(10) UNSIGNED DEFAULT NULL,
  `l_df` int(10) UNSIGNED DEFAULT NULL,
  `l_svpt` int(10) UNSIGNED DEFAULT NULL,
  `l_1stIn` int(10) UNSIGNED DEFAULT NULL,
  `l_1stWon` int(10) UNSIGNED DEFAULT NULL,
  `l_2ndWon` int(10) UNSIGNED DEFAULT NULL,
  `l_SvGms` int(10) UNSIGNED DEFAULT NULL,
  `l_bpSaved` int(10) UNSIGNED DEFAULT NULL,
  `l_bpFaced` int(10) UNSIGNED DEFAULT NULL,
  `w_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `w_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_1` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_2` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_3` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_4` tinyint(3) UNSIGNED DEFAULT NULL,
  `l_set_5` tinyint(3) UNSIGNED DEFAULT NULL,
  `tb_set_1` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_2` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_3` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_4` smallint(5) UNSIGNED DEFAULT NULL,
  `tb_set_5` smallint(5) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `players` (
  `ID` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  `nationality` char(3) COLLATE utf8_bin NOT NULL,
  `hand` char(1) COLLATE utf8_bin DEFAULT NULL,
  `height` int(10) UNSIGNED DEFAULT NULL,
  `date_of_birth` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `players_nat_history` (
  `ID` bigint(20) UNSIGNED NOT NULL,
  `nationality` char(3) COLLATE utf8_bin NOT NULL,
  `date_end` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `points` (
  `level_ID` tinyint(3) UNSIGNED NOT NULL,
  `round_ID` tinyint(3) UNSIGNED NOT NULL,
  `points_w` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `points_l` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `points_l_ex` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `is_cumuled` tinyint(1) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `rounds` (
  `ID` tinyint(3) UNSIGNED NOT NULL,
  `original_code` varchar(5) COLLATE utf8_bin NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `surfaces` (
  `ID` tinyint(3) UNSIGNED NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `tournament` (
  `ID` int(10) UNSIGNED NOT NULL,
  `original_code` varchar(5) COLLATE utf8_bin NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  `city` varchar(255) COLLATE utf8_bin NOT NULL,
  `level_ID` tinyint(3) UNSIGNED NOT NULL,
  `surface_ID` tinyint(3) UNSIGNED NOT NULL,
  `is_indoor` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `slot_order` tinyint(3) UNSIGNED DEFAULT NULL,
  `last_year` int(10) UNSIGNED DEFAULT NULL,
  `substitute_ID` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


ALTER TABLE `backup_matches`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `original_key` (`original_key`),
  ADD KEY `winner_id` (`winner_ID`),
  ADD KEY `loser_id` (`loser_ID`),
  ADD KEY `surface_id` (`surface_id`),
  ADD KEY `tourney_level_id` (`tourney_level_id`),
  ADD KEY `tourney_year` (`tourney_year`);

ALTER TABLE `backup_matches_davis`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `original_key` (`original_key`),
  ADD KEY `winner_id` (`winner_ID`),
  ADD KEY `loser_id` (`loser_ID`),
  ADD KEY `surface_id` (`surface_id`),
  ADD KEY `tourney_level_id` (`tourney_level_id`),
  ADD KEY `tourney_year` (`tourney_year`);

ALTER TABLE `backup_tourneys_indoor_history`
  ADD PRIMARY KEY (`ID`,`last_year`);

ALTER TABLE `backup_tourneys_level_history`
  ADD PRIMARY KEY (`ID`,`last_year`),
  ADD KEY `level_ID` (`level_ID`),
  ADD KEY `substitute_ID` (`substitute_ID`);

ALTER TABLE `backup_tourneys_name_and_city_history`
  ADD PRIMARY KEY (`ID`,`last_year`);

ALTER TABLE `backup_tourneys_slot_history`
  ADD PRIMARY KEY (`ID`,`last_year`),
  ADD KEY `ID` (`ID`);

ALTER TABLE `backup_tourneys_surface_history`
  ADD PRIMARY KEY (`ID`,`last_year`),
  ADD KEY `surface_ID` (`surface_ID`);

ALTER TABLE `editions`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `tournament_year` (`tournament_ID`,`year`) USING BTREE,
  ADD KEY `surface_ID` (`surface_ID`),
  ADD KEY `level_ID` (`level_ID`),
  ADD KEY `substitute_ID` (`substitute_ID`);

ALTER TABLE `levels`
  ADD PRIMARY KEY (`ID`);

ALTER TABLE `matches`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `original_key` (`original_key`),
  ADD KEY `winner_id` (`winner_ID`),
  ADD KEY `loser_id` (`loser_ID`),
  ADD KEY `edition_ID` (`edition_ID`),
  ADD KEY `round` (`round_ID`);

ALTER TABLE `players`
  ADD PRIMARY KEY (`ID`);

ALTER TABLE `players_nat_history`
  ADD PRIMARY KEY (`ID`,`date_end`),
  ADD KEY `ID` (`ID`);

ALTER TABLE `points`
  ADD PRIMARY KEY (`level_ID`,`round_ID`),
  ADD KEY `round_ID` (`round_ID`);

ALTER TABLE `rounds`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `original_code` (`original_code`);

ALTER TABLE `surfaces`
  ADD PRIMARY KEY (`ID`);

ALTER TABLE `tournament`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `level_ID` (`level_ID`),
  ADD KEY `surface_ID` (`surface_ID`),
  ADD KEY `substitute_ID` (`substitute_ID`);


ALTER TABLE `backup_matches`
  MODIFY `ID` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `backup_matches_davis`
  MODIFY `ID` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `editions`
  MODIFY `ID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `matches`
  MODIFY `ID` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `rounds`
  MODIFY `ID` tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `surfaces`
  MODIFY `ID` tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT;
ALTER TABLE `tournament`
  MODIFY `ID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

ALTER TABLE `editions`
  ADD CONSTRAINT `editions_ibfk_1` FOREIGN KEY (`tournament_ID`) REFERENCES `tournament` (`ID`),
  ADD CONSTRAINT `editions_ibfk_2` FOREIGN KEY (`surface_ID`) REFERENCES `surfaces` (`ID`),
  ADD CONSTRAINT `editions_ibfk_3` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`),
  ADD CONSTRAINT `editions_ibfk_4` FOREIGN KEY (`substitute_ID`) REFERENCES `tournament` (`ID`);

ALTER TABLE `matches`
  ADD CONSTRAINT `matches_ibfk_2` FOREIGN KEY (`winner_ID`) REFERENCES `players` (`ID`),
  ADD CONSTRAINT `matches_ibfk_3` FOREIGN KEY (`loser_ID`) REFERENCES `players` (`ID`),
  ADD CONSTRAINT `matches_ibfk_4` FOREIGN KEY (`edition_ID`) REFERENCES `editions` (`ID`),
  ADD CONSTRAINT `matches_ibfk_5` FOREIGN KEY (`round_ID`) REFERENCES `rounds` (`ID`);

ALTER TABLE `players_nat_history`
  ADD CONSTRAINT `players_nat_history_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `players` (`ID`);

ALTER TABLE `points`
  ADD CONSTRAINT `points_ibfk_1` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`),
  ADD CONSTRAINT `points_ibfk_2` FOREIGN KEY (`round_ID`) REFERENCES `rounds` (`ID`);

ALTER TABLE `tournament`
  ADD CONSTRAINT `tournament_ibfk_3` FOREIGN KEY (`substitute_ID`) REFERENCES `tournament` (`ID`),
  ADD CONSTRAINT `tournament_ibfk_4` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`),
  ADD CONSTRAINT `tournament_ibfk_5` FOREIGN KEY (`surface_ID`) REFERENCES `surfaces` (`ID`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
