SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

CREATE TABLE atp_ranking (
  player_ID bigint(20) UNSIGNED NOT NULL,
  year int(10) UNSIGNED NOT NULL,
  week_no int(10) UNSIGNED NOT NULL,
  week_points int(10) UNSIGNED NOT NULL,
  year_calendar_points int(10) UNSIGNED NOT NULL,
  year_rolling_points int(10) UNSIGNED NOT NULL,
  tournaments_concat varchar(255) COLLATE utf8_bin NOT NULL,
  tournaments_calendar_concat varchar(255) COLLATE utf8_bin NOT NULL,
  tournaments_rolling_concat varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE countries (
  code_ISO2 char(2) COLLATE utf8_bin NOT NULL,
  code_ISO3 char(3) COLLATE utf8_bin NOT NULL,
  name_EN varchar(100) COLLATE utf8_bin NOT NULL,
  name_FR varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE editions (
  ID int(10) UNSIGNED NOT NULL,
  tournament_ID int(10) UNSIGNED NOT NULL,
  year int(10) UNSIGNED NOT NULL,
  draw_size smallint(5) UNSIGNED NOT NULL,
  date_begin datetime NOT NULL,
  date_end datetime NOT NULL,
  surface_ID tinyint(3) UNSIGNED NOT NULL,
  slot_order tinyint(3) UNSIGNED DEFAULT NULL,
  is_indoor tinyint(1) UNSIGNED NOT NULL,
  level_ID tinyint(3) UNSIGNED NOT NULL,
  substitute_ID int(10) UNSIGNED DEFAULT NULL,
  name varchar(255) COLLATE utf8_bin DEFAULT NULL,
  city varchar(255) COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE edition_player_stats (
  edition_ID int(10) UNSIGNED NOT NULL,
  player_ID bigint(20) UNSIGNED NOT NULL,
  round_ID tinyint(3) UNSIGNED NOT NULL,
  is_winner tinyint(1) UNSIGNED NOT NULL,
  points int(10) UNSIGNED NOT NULL DEFAULT '0',
  count_match_win smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_match_lost smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_set_win smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_set_lost smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_game_win smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_game_lost smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_tb_win smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_tb_lost smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_ace smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_df smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_svpt smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_1stIn smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_1stWon smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_2ndWon smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_SvGms smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_bpSaved smallint(5) UNSIGNED NOT NULL DEFAULT '0',
  count_bpFaced smallint(5) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE levels (
  ID tinyint(3) UNSIGNED NOT NULL,
  name varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE matches (
  ID bigint(20) UNSIGNED NOT NULL,
  original_key varchar(255) COLLATE utf8_bin NOT NULL,
  edition_ID int(10) UNSIGNED NOT NULL,
  match_num smallint(5) UNSIGNED NOT NULL,
  round_ID tinyint(3) UNSIGNED NOT NULL,
  best_of tinyint(3) UNSIGNED NOT NULL,
  winner_ID bigint(20) UNSIGNED NOT NULL,
  winner_seed int(10) UNSIGNED DEFAULT NULL,
  winner_entry varchar(5) COLLATE utf8_bin DEFAULT NULL,
  winner_rank int(10) UNSIGNED DEFAULT NULL,
  winner_rank_points int(10) UNSIGNED DEFAULT NULL,
  loser_ID bigint(20) UNSIGNED NOT NULL,
  loser_seed int(10) UNSIGNED DEFAULT NULL,
  loser_entry varchar(5) COLLATE utf8_bin DEFAULT NULL,
  loser_rank int(10) UNSIGNED DEFAULT NULL,
  loser_rank_points int(10) UNSIGNED DEFAULT NULL,
  minutes int(10) UNSIGNED DEFAULT NULL,
  unfinished tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  retirement tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  walkover tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  w_ace int(10) UNSIGNED DEFAULT NULL,
  w_df int(10) UNSIGNED DEFAULT NULL,
  w_svpt int(10) UNSIGNED DEFAULT NULL,
  w_1stIn int(10) UNSIGNED DEFAULT NULL,
  w_1stWon int(10) UNSIGNED DEFAULT NULL,
  w_2ndWon int(10) UNSIGNED DEFAULT NULL,
  w_SvGms int(10) UNSIGNED DEFAULT NULL,
  w_bpSaved int(10) UNSIGNED DEFAULT NULL,
  w_bpFaced int(10) UNSIGNED DEFAULT NULL,
  l_ace int(10) UNSIGNED DEFAULT NULL,
  l_df int(10) UNSIGNED DEFAULT NULL,
  l_svpt int(10) UNSIGNED DEFAULT NULL,
  l_1stIn int(10) UNSIGNED DEFAULT NULL,
  l_1stWon int(10) UNSIGNED DEFAULT NULL,
  l_2ndWon int(10) UNSIGNED DEFAULT NULL,
  l_SvGms int(10) UNSIGNED DEFAULT NULL,
  l_bpSaved int(10) UNSIGNED DEFAULT NULL,
  l_bpFaced int(10) UNSIGNED DEFAULT NULL,
  w_set_1 tinyint(3) UNSIGNED DEFAULT NULL,
  w_set_2 tinyint(3) UNSIGNED DEFAULT NULL,
  w_set_3 tinyint(3) UNSIGNED DEFAULT NULL,
  w_set_4 tinyint(3) UNSIGNED DEFAULT NULL,
  w_set_5 tinyint(3) UNSIGNED DEFAULT NULL,
  l_set_1 tinyint(3) UNSIGNED DEFAULT NULL,
  l_set_2 tinyint(3) UNSIGNED DEFAULT NULL,
  l_set_3 tinyint(3) UNSIGNED DEFAULT NULL,
  l_set_4 tinyint(3) UNSIGNED DEFAULT NULL,
  l_set_5 tinyint(3) UNSIGNED DEFAULT NULL,
  tb_set_1 smallint(5) UNSIGNED DEFAULT NULL,
  tb_set_2 smallint(5) UNSIGNED DEFAULT NULL,
  tb_set_3 smallint(5) UNSIGNED DEFAULT NULL,
  tb_set_4 smallint(5) UNSIGNED DEFAULT NULL,
  tb_set_5 smallint(5) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE players (
  ID bigint(20) UNSIGNED NOT NULL,
  name varchar(255) COLLATE utf8_bin NOT NULL,
  nationality char(3) COLLATE utf8_bin NOT NULL,
  hand char(1) COLLATE utf8_bin DEFAULT NULL,
  height int(10) UNSIGNED DEFAULT NULL,
  date_of_birth datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE players_nat_history (
  ID bigint(20) UNSIGNED NOT NULL,
  nationality char(3) COLLATE utf8_bin NOT NULL,
  date_end datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE points (
  level_ID tinyint(3) UNSIGNED NOT NULL,
  round_ID tinyint(3) UNSIGNED NOT NULL,
  points_w int(10) UNSIGNED NOT NULL DEFAULT '0',
  points_l int(10) UNSIGNED NOT NULL DEFAULT '0',
  points_l_ex int(10) UNSIGNED NOT NULL DEFAULT '0',
  is_cumuled tinyint(1) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE rounds (
  ID tinyint(3) UNSIGNED NOT NULL,
  original_code varchar(5) COLLATE utf8_bin NOT NULL,
  name varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE surfaces (
  ID tinyint(3) UNSIGNED NOT NULL,
  name varchar(255) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE tournaments (
  ID int(10) UNSIGNED NOT NULL,
  original_code varchar(5) COLLATE utf8_bin NOT NULL,
  name varchar(255) COLLATE utf8_bin NOT NULL,
  city varchar(255) COLLATE utf8_bin NOT NULL,
  level_ID tinyint(3) UNSIGNED NOT NULL,
  surface_ID tinyint(3) UNSIGNED NOT NULL,
  is_indoor tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  slot_order tinyint(3) UNSIGNED DEFAULT NULL,
  last_year int(10) UNSIGNED DEFAULT NULL,
  substitute_ID int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE tournaments_code_2016 (
  code_original varchar(5) COLLATE utf8_bin NOT NULL,
  code_new varchar(5) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE tournaments_code_2017 (
  code_original varchar(5) COLLATE utf8_bin NOT NULL,
  code_new varchar(5) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


ALTER TABLE atp_ranking
  ADD PRIMARY KEY (player_ID,year,week_no),
  ADD KEY player_ID (player_ID);

ALTER TABLE countries
  ADD PRIMARY KEY (code_ISO2),
  ADD UNIQUE KEY code_ISO3 (code_ISO3);

ALTER TABLE editions
  ADD PRIMARY KEY (ID),
  ADD UNIQUE KEY tournament_year (tournament_ID,year) USING BTREE,
  ADD KEY surface_ID (surface_ID),
  ADD KEY level_ID (level_ID),
  ADD KEY substitute_ID (substitute_ID);

ALTER TABLE edition_player_stats
  ADD PRIMARY KEY (edition_ID,player_ID),
  ADD KEY edition_ID (edition_ID),
  ADD KEY player_ID (player_ID) USING BTREE,
  ADD KEY round_ID (round_ID);

ALTER TABLE levels
  ADD PRIMARY KEY (ID);

ALTER TABLE matches
  ADD PRIMARY KEY (ID),
  ADD UNIQUE KEY original_key (original_key),
  ADD KEY winner_id (winner_ID),
  ADD KEY loser_id (loser_ID),
  ADD KEY edition_ID (edition_ID),
  ADD KEY round (round_ID);

ALTER TABLE players
  ADD PRIMARY KEY (ID),
  ADD KEY nationality (nationality);

ALTER TABLE players_nat_history
  ADD PRIMARY KEY (ID,date_end),
  ADD KEY ID (ID),
  ADD KEY nationality (nationality);

ALTER TABLE points
  ADD PRIMARY KEY (level_ID,round_ID),
  ADD KEY round_ID (round_ID),
  ADD KEY level_ID (level_ID);

ALTER TABLE rounds
  ADD PRIMARY KEY (ID),
  ADD KEY original_code (original_code);

ALTER TABLE surfaces
  ADD PRIMARY KEY (ID);

ALTER TABLE tournaments
  ADD PRIMARY KEY (ID),
  ADD KEY level_ID (level_ID),
  ADD KEY surface_ID (surface_ID),
  ADD KEY substitute_ID (substitute_ID);

ALTER TABLE tournaments_code_2016
  ADD PRIMARY KEY (code_original),
  ADD KEY code_new (code_new);

ALTER TABLE tournaments_code_2017
  ADD PRIMARY KEY (code_original),
  ADD KEY code_new (code_new);


ALTER TABLE editions
  MODIFY ID int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3765;
ALTER TABLE matches
  MODIFY ID bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=166865;
ALTER TABLE rounds
  MODIFY ID tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
ALTER TABLE surfaces
  MODIFY ID tinyint(3) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
ALTER TABLE tournaments
  MODIFY ID int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=362;

ALTER TABLE atp_ranking
  ADD CONSTRAINT atp_ranking_ibfk_1 FOREIGN KEY (player_ID) REFERENCES players (ID);

ALTER TABLE editions
  ADD CONSTRAINT editions_ibfk_1 FOREIGN KEY (tournament_ID) REFERENCES tournaments (ID),
  ADD CONSTRAINT editions_ibfk_2 FOREIGN KEY (surface_ID) REFERENCES surfaces (ID),
  ADD CONSTRAINT editions_ibfk_3 FOREIGN KEY (level_ID) REFERENCES `levels` (ID),
  ADD CONSTRAINT editions_ibfk_4 FOREIGN KEY (substitute_ID) REFERENCES tournaments (ID);

ALTER TABLE edition_player_stats
  ADD CONSTRAINT edition_player_stats_ibfk_1 FOREIGN KEY (edition_ID) REFERENCES editions (ID),
  ADD CONSTRAINT edition_player_stats_ibfk_2 FOREIGN KEY (player_ID) REFERENCES players (ID),
  ADD CONSTRAINT edition_player_stats_ibfk_3 FOREIGN KEY (round_ID) REFERENCES rounds (ID);

ALTER TABLE matches
  ADD CONSTRAINT matches_ibfk_2 FOREIGN KEY (winner_ID) REFERENCES players (ID),
  ADD CONSTRAINT matches_ibfk_3 FOREIGN KEY (loser_ID) REFERENCES players (ID),
  ADD CONSTRAINT matches_ibfk_4 FOREIGN KEY (edition_ID) REFERENCES editions (ID),
  ADD CONSTRAINT matches_ibfk_5 FOREIGN KEY (round_ID) REFERENCES rounds (ID);

ALTER TABLE players_nat_history
  ADD CONSTRAINT players_nat_history_ibfk_1 FOREIGN KEY (ID) REFERENCES players (ID);

ALTER TABLE points
  ADD CONSTRAINT points_ibfk_1 FOREIGN KEY (level_ID) REFERENCES `levels` (ID),
  ADD CONSTRAINT points_ibfk_2 FOREIGN KEY (round_ID) REFERENCES rounds (ID);

ALTER TABLE tournaments
  ADD CONSTRAINT tournaments_ibfk_3 FOREIGN KEY (substitute_ID) REFERENCES tournaments (ID),
  ADD CONSTRAINT tournaments_ibfk_4 FOREIGN KEY (level_ID) REFERENCES `levels` (ID),
  ADD CONSTRAINT tournaments_ibfk_5 FOREIGN KEY (surface_ID) REFERENCES surfaces (ID);
