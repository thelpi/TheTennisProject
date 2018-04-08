SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
CREATE DATABASE IF NOT EXISTS `the_tennis_project` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `the_tennis_project`;

DROP TABLE IF EXISTS `editions`;
CREATE TABLE IF NOT EXISTS `editions` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `tourney_ID` int(10) unsigned NOT NULL,
  `year` int(10) unsigned NOT NULL,
  `draw_size` smallint(5) unsigned NOT NULL,
  `date_begin` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `tourney_ID` (`tourney_ID`,`year`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=3631 ;

DROP TABLE IF EXISTS `levels`;
CREATE TABLE IF NOT EXISTS `levels` (
  `ID` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `matches`;
CREATE TABLE IF NOT EXISTS `matches` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `edition_ID` int(10) unsigned NOT NULL,
  `match_num` smallint(5) unsigned NOT NULL,
  `round_ID` tinyint(3) unsigned NOT NULL,
  `best_of` tinyint(3) unsigned NOT NULL,
  `winner_ID` bigint(20) unsigned NOT NULL,
  `winner_seed` int(10) unsigned DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) unsigned DEFAULT NULL,
  `winner_rank_points` int(10) unsigned DEFAULT NULL,
  `loser_ID` bigint(20) unsigned NOT NULL,
  `loser_seed` int(10) unsigned DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) unsigned DEFAULT NULL,
  `loser_rank_points` int(10) unsigned DEFAULT NULL,
  `minutes` int(10) unsigned DEFAULT NULL,
  `unfinished` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `retirement` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `walkover` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `w_ace` int(10) unsigned DEFAULT NULL,
  `w_df` int(10) unsigned DEFAULT NULL,
  `w_svpt` int(10) unsigned DEFAULT NULL,
  `w_1stIn` int(10) unsigned DEFAULT NULL,
  `w_1stWon` int(10) unsigned DEFAULT NULL,
  `w_2ndWon` int(10) unsigned DEFAULT NULL,
  `w_SvGms` int(10) unsigned DEFAULT NULL,
  `w_bpSaved` int(10) unsigned DEFAULT NULL,
  `w_bpFaced` int(10) unsigned DEFAULT NULL,
  `l_ace` int(10) unsigned DEFAULT NULL,
  `l_df` int(10) unsigned DEFAULT NULL,
  `l_svpt` int(10) unsigned DEFAULT NULL,
  `l_1stIn` int(10) unsigned DEFAULT NULL,
  `l_1stWon` int(10) unsigned DEFAULT NULL,
  `l_2ndWon` int(10) unsigned DEFAULT NULL,
  `l_SvGms` int(10) unsigned DEFAULT NULL,
  `l_bpSaved` int(10) unsigned DEFAULT NULL,
  `l_bpFaced` int(10) unsigned DEFAULT NULL,
  `w_set_1` tinyint(3) unsigned DEFAULT NULL,
  `w_set_2` tinyint(3) unsigned DEFAULT NULL,
  `w_set_3` tinyint(3) unsigned DEFAULT NULL,
  `w_set_4` tinyint(3) unsigned DEFAULT NULL,
  `w_set_5` tinyint(3) unsigned DEFAULT NULL,
  `l_set_1` tinyint(3) unsigned DEFAULT NULL,
  `l_set_2` tinyint(3) unsigned DEFAULT NULL,
  `l_set_3` tinyint(3) unsigned DEFAULT NULL,
  `l_set_4` tinyint(3) unsigned DEFAULT NULL,
  `l_set_5` tinyint(3) unsigned DEFAULT NULL,
  `tb_set_1` smallint(5) unsigned DEFAULT NULL,
  `tb_set_2` smallint(5) unsigned DEFAULT NULL,
  `tb_set_3` smallint(5) unsigned DEFAULT NULL,
  `tb_set_4` smallint(5) unsigned DEFAULT NULL,
  `tb_set_5` smallint(5) unsigned DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `original_key` (`original_key`),
  KEY `winner_id` (`winner_ID`),
  KEY `loser_id` (`loser_ID`),
  KEY `edition_ID` (`edition_ID`),
  KEY `round` (`round_ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=161541 ;

DROP TABLE IF EXISTS `matches_bkp`;
CREATE TABLE IF NOT EXISTS `matches_bkp` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_year` int(11) DEFAULT NULL,
  `tourney_id` varchar(5) COLLATE utf8_bin NOT NULL,
  `tourney_name` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_level_id` tinyint(3) unsigned NOT NULL,
  `tourney_date` datetime NOT NULL,
  `surface_id` tinyint(3) unsigned DEFAULT NULL,
  `draw_size` smallint(6) NOT NULL,
  `match_num` smallint(5) unsigned DEFAULT NULL,
  `winner_ID` bigint(20) unsigned NOT NULL,
  `winner_seed` int(10) unsigned DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) unsigned DEFAULT NULL,
  `winner_rank_points` int(10) unsigned DEFAULT NULL,
  `loser_ID` bigint(20) unsigned NOT NULL,
  `loser_seed` int(10) unsigned DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) unsigned DEFAULT NULL,
  `loser_rank_points` int(10) unsigned DEFAULT NULL,
  `round` varchar(5) COLLATE utf8_bin NOT NULL,
  `best_of` tinyint(3) unsigned NOT NULL,
  `minutes` int(10) unsigned DEFAULT NULL,
  `unfinished` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `retirement` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `walkover` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `w_ace` int(10) unsigned DEFAULT NULL,
  `w_df` int(10) unsigned DEFAULT NULL,
  `w_svpt` int(10) unsigned DEFAULT NULL,
  `w_1stIn` int(10) unsigned DEFAULT NULL,
  `w_1stWon` int(10) unsigned DEFAULT NULL,
  `w_2ndWon` int(10) unsigned DEFAULT NULL,
  `w_SvGms` int(10) unsigned DEFAULT NULL,
  `w_bpSaved` int(10) unsigned DEFAULT NULL,
  `w_bpFaced` int(10) unsigned DEFAULT NULL,
  `l_ace` int(10) unsigned DEFAULT NULL,
  `l_df` int(10) unsigned DEFAULT NULL,
  `l_svpt` int(10) unsigned DEFAULT NULL,
  `l_1stIn` int(10) unsigned DEFAULT NULL,
  `l_1stWon` int(10) unsigned DEFAULT NULL,
  `l_2ndWon` int(10) unsigned DEFAULT NULL,
  `l_SvGms` int(10) unsigned DEFAULT NULL,
  `l_bpSaved` int(10) unsigned DEFAULT NULL,
  `l_bpFaced` int(10) unsigned DEFAULT NULL,
  `w_set_1` tinyint(3) unsigned DEFAULT NULL,
  `w_set_2` tinyint(3) unsigned DEFAULT NULL,
  `w_set_3` tinyint(3) unsigned DEFAULT NULL,
  `w_set_4` tinyint(3) unsigned DEFAULT NULL,
  `w_set_5` tinyint(3) unsigned DEFAULT NULL,
  `l_set_1` tinyint(3) unsigned DEFAULT NULL,
  `l_set_2` tinyint(3) unsigned DEFAULT NULL,
  `l_set_3` tinyint(3) unsigned DEFAULT NULL,
  `l_set_4` tinyint(3) unsigned DEFAULT NULL,
  `l_set_5` tinyint(3) unsigned DEFAULT NULL,
  `tb_set_1` smallint(5) unsigned DEFAULT NULL,
  `tb_set_2` smallint(5) unsigned DEFAULT NULL,
  `tb_set_3` smallint(5) unsigned DEFAULT NULL,
  `tb_set_4` smallint(5) unsigned DEFAULT NULL,
  `tb_set_5` smallint(5) unsigned DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `original_key` (`original_key`),
  KEY `winner_id` (`winner_ID`),
  KEY `loser_id` (`loser_ID`),
  KEY `surface_id` (`surface_id`),
  KEY `tourney_level_id` (`tourney_level_id`),
  KEY `tourney_year` (`tourney_year`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=161556 ;

DROP TABLE IF EXISTS `matches_davis`;
CREATE TABLE IF NOT EXISTS `matches_davis` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `original_key` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_year` int(11) DEFAULT NULL,
  `tourney_id` varchar(5) COLLATE utf8_bin NOT NULL,
  `tourney_name` varchar(255) COLLATE utf8_bin NOT NULL,
  `tourney_level_id` tinyint(3) unsigned NOT NULL,
  `tourney_date` datetime NOT NULL,
  `surface_id` tinyint(3) unsigned DEFAULT NULL,
  `draw_size` smallint(6) NOT NULL,
  `match_num` smallint(5) unsigned DEFAULT NULL,
  `winner_ID` bigint(20) unsigned NOT NULL,
  `winner_seed` int(10) unsigned DEFAULT NULL,
  `winner_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `winner_rank` int(10) unsigned DEFAULT NULL,
  `winner_rank_points` int(10) unsigned DEFAULT NULL,
  `loser_ID` bigint(20) unsigned NOT NULL,
  `loser_seed` int(10) unsigned DEFAULT NULL,
  `loser_entry` varchar(5) COLLATE utf8_bin DEFAULT NULL,
  `loser_rank` int(10) unsigned DEFAULT NULL,
  `loser_rank_points` int(10) unsigned DEFAULT NULL,
  `round` varchar(5) COLLATE utf8_bin NOT NULL,
  `best_of` tinyint(3) unsigned NOT NULL,
  `minutes` int(10) unsigned DEFAULT NULL,
  `unfinished` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `retirement` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `walkover` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `w_ace` int(10) unsigned DEFAULT NULL,
  `w_df` int(10) unsigned DEFAULT NULL,
  `w_svpt` int(10) unsigned DEFAULT NULL,
  `w_1stIn` int(10) unsigned DEFAULT NULL,
  `w_1stWon` int(10) unsigned DEFAULT NULL,
  `w_2ndWon` int(10) unsigned DEFAULT NULL,
  `w_SvGms` int(10) unsigned DEFAULT NULL,
  `w_bpSaved` int(10) unsigned DEFAULT NULL,
  `w_bpFaced` int(10) unsigned DEFAULT NULL,
  `l_ace` int(10) unsigned DEFAULT NULL,
  `l_df` int(10) unsigned DEFAULT NULL,
  `l_svpt` int(10) unsigned DEFAULT NULL,
  `l_1stIn` int(10) unsigned DEFAULT NULL,
  `l_1stWon` int(10) unsigned DEFAULT NULL,
  `l_2ndWon` int(10) unsigned DEFAULT NULL,
  `l_SvGms` int(10) unsigned DEFAULT NULL,
  `l_bpSaved` int(10) unsigned DEFAULT NULL,
  `l_bpFaced` int(10) unsigned DEFAULT NULL,
  `w_set_1` tinyint(3) unsigned DEFAULT NULL,
  `w_set_2` tinyint(3) unsigned DEFAULT NULL,
  `w_set_3` tinyint(3) unsigned DEFAULT NULL,
  `w_set_4` tinyint(3) unsigned DEFAULT NULL,
  `w_set_5` tinyint(3) unsigned DEFAULT NULL,
  `l_set_1` tinyint(3) unsigned DEFAULT NULL,
  `l_set_2` tinyint(3) unsigned DEFAULT NULL,
  `l_set_3` tinyint(3) unsigned DEFAULT NULL,
  `l_set_4` tinyint(3) unsigned DEFAULT NULL,
  `l_set_5` tinyint(3) unsigned DEFAULT NULL,
  `tb_set_1` smallint(5) unsigned DEFAULT NULL,
  `tb_set_2` smallint(5) unsigned DEFAULT NULL,
  `tb_set_3` smallint(5) unsigned DEFAULT NULL,
  `tb_set_4` smallint(5) unsigned DEFAULT NULL,
  `tb_set_5` smallint(5) unsigned DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `original_key` (`original_key`),
  KEY `winner_id` (`winner_ID`),
  KEY `loser_id` (`loser_ID`),
  KEY `surface_id` (`surface_id`),
  KEY `tourney_level_id` (`tourney_level_id`),
  KEY `tourney_year` (`tourney_year`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=161869 ;

DROP TABLE IF EXISTS `players`;
CREATE TABLE IF NOT EXISTS `players` (
  `ID` bigint(20) unsigned NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  `nationality` char(3) COLLATE utf8_bin NOT NULL,
  `hand` char(1) COLLATE utf8_bin DEFAULT NULL,
  `height` int(10) unsigned DEFAULT NULL,
  `date_of_birth` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `players_nat_history`;
CREATE TABLE IF NOT EXISTS `players_nat_history` (
  `ID` bigint(20) unsigned NOT NULL,
  `nationality` char(3) COLLATE utf8_bin NOT NULL,
  `date_end` datetime NOT NULL,
  PRIMARY KEY (`ID`,`date_end`),
  KEY `ID` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `points`;
CREATE TABLE IF NOT EXISTS `points` (
  `level_ID` tinyint(3) unsigned NOT NULL,
  `round_ID` tinyint(3) unsigned NOT NULL,
  `points_w` int(10) unsigned NOT NULL DEFAULT '0',
  `points_l` int(10) unsigned NOT NULL DEFAULT '0',
  `points_l_ex` int(10) unsigned NOT NULL DEFAULT '0',
  `is_cumuled` tinyint(1) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`level_ID`,`round_ID`),
  KEY `round_ID` (`round_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `rounds`;
CREATE TABLE IF NOT EXISTS `rounds` (
  `ID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `original_code` varchar(5) COLLATE utf8_bin NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `original_code` (`original_code`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=10 ;

DROP TABLE IF EXISTS `surfaces`;
CREATE TABLE IF NOT EXISTS `surfaces` (
  `ID` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=6 ;

DROP TABLE IF EXISTS `tourneys`;
CREATE TABLE IF NOT EXISTS `tourneys` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `original_code` varchar(5) COLLATE utf8_bin NOT NULL,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  `city` varchar(255) COLLATE utf8_bin NOT NULL,
  `level_ID` tinyint(3) unsigned NOT NULL,
  `surface_ID` tinyint(3) unsigned NOT NULL,
  `is_indoor` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `slot_order` tinyint(3) unsigned DEFAULT NULL,
  `last_year` int(10) unsigned DEFAULT NULL,
  `substitute_ID` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `level_ID` (`level_ID`),
  KEY `surface_ID` (`surface_ID`),
  KEY `substitute_ID` (`substitute_ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_bin AUTO_INCREMENT=354 ;

DROP TABLE IF EXISTS `tourneys_indoor_history`;
CREATE TABLE IF NOT EXISTS `tourneys_indoor_history` (
  `ID` int(10) unsigned NOT NULL,
  `is_indoor` tinyint(1) unsigned NOT NULL,
  `last_year` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`last_year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `tourneys_level_history`;
CREATE TABLE IF NOT EXISTS `tourneys_level_history` (
  `ID` int(10) unsigned NOT NULL,
  `level_ID` tinyint(3) unsigned NOT NULL,
  `last_year` int(10) unsigned NOT NULL,
  `substitute_ID` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`ID`,`last_year`),
  KEY `level_ID` (`level_ID`),
  KEY `substitute_ID` (`substitute_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `tourneys_name_and_city_history`;
CREATE TABLE IF NOT EXISTS `tourneys_name_and_city_history` (
  `ID` int(10) unsigned NOT NULL,
  `name` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `city` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `last_year` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`last_year`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `tourneys_slot_history`;
CREATE TABLE IF NOT EXISTS `tourneys_slot_history` (
  `ID` int(10) unsigned NOT NULL,
  `slot_order` tinyint(3) unsigned NOT NULL,
  `last_year` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`last_year`),
  KEY `ID` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

DROP TABLE IF EXISTS `tourneys_surface_history`;
CREATE TABLE IF NOT EXISTS `tourneys_surface_history` (
  `ID` int(10) unsigned NOT NULL,
  `surface_ID` tinyint(3) unsigned NOT NULL,
  `last_year` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`last_year`),
  KEY `surface_ID` (`surface_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


ALTER TABLE `editions`
  ADD CONSTRAINT `editions_ibfk_1` FOREIGN KEY (`tourney_ID`) REFERENCES `tourneys` (`ID`);

ALTER TABLE `matches`
  ADD CONSTRAINT `matches_ibfk_2` FOREIGN KEY (`winner_ID`) REFERENCES `players` (`id`),
  ADD CONSTRAINT `matches_ibfk_3` FOREIGN KEY (`loser_ID`) REFERENCES `players` (`id`),
  ADD CONSTRAINT `matches_ibfk_4` FOREIGN KEY (`edition_ID`) REFERENCES `editions` (`ID`),
  ADD CONSTRAINT `matches_ibfk_5` FOREIGN KEY (`round_ID`) REFERENCES `rounds` (`ID`);

ALTER TABLE `players_nat_history`
  ADD CONSTRAINT `players_nat_history_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `players` (`id`);

ALTER TABLE `points`
  ADD CONSTRAINT `points_ibfk_1` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`),
  ADD CONSTRAINT `points_ibfk_2` FOREIGN KEY (`round_ID`) REFERENCES `rounds` (`ID`);

ALTER TABLE `tourneys`
  ADD CONSTRAINT `tourneys_ibfk_3` FOREIGN KEY (`substitute_ID`) REFERENCES `tourneys` (`ID`),
  ADD CONSTRAINT `tourneys_ibfk_4` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`),
  ADD CONSTRAINT `tourneys_ibfk_5` FOREIGN KEY (`surface_ID`) REFERENCES `surfaces` (`ID`);

ALTER TABLE `tourneys_indoor_history`
  ADD CONSTRAINT `tourneys_indoor_history_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `tourneys` (`ID`);

ALTER TABLE `tourneys_level_history`
  ADD CONSTRAINT `tourneys_level_history_ibfk_2` FOREIGN KEY (`ID`) REFERENCES `tourneys` (`ID`),
  ADD CONSTRAINT `tourneys_level_history_ibfk_3` FOREIGN KEY (`substitute_ID`) REFERENCES `tourneys` (`ID`),
  ADD CONSTRAINT `tourneys_level_history_ibfk_4` FOREIGN KEY (`level_ID`) REFERENCES `levels` (`ID`);

ALTER TABLE `tourneys_name_and_city_history`
  ADD CONSTRAINT `tourneys_name_and_city_history_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `tourneys` (`ID`);

ALTER TABLE `tourneys_slot_history`
  ADD CONSTRAINT `tourneys_slot_history_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `tourneys` (`ID`);

ALTER TABLE `tourneys_surface_history`
  ADD CONSTRAINT `tourneys_surface_history_ibfk_3` FOREIGN KEY (`ID`) REFERENCES `tourneys` (`ID`),
  ADD CONSTRAINT `tourneys_surface_history_ibfk_4` FOREIGN KEY (`surface_ID`) REFERENCES `surfaces` (`ID`);
