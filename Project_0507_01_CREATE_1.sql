USE BUDT703_Project_0507_01

DROP TABLE IF EXISTS [CurveballConsultants.Match]
DROP TABLE IF EXISTS [CurveBallConsultants.Season]
DROP TABLE IF EXISTS [CurveBallConsultants.Location]
DROP TABLE IF EXISTS [CurveBallConsultants.Tournament]
DROP TABLE IF EXISTS [CurveBallConsultants.Opponent]

CREATE TABLE [CurveBallConsultants.Opponent] (
	oppId CHAR (3) NOT NULL,
	oppName VARCHAR (40),
	CONSTRAINT pk_Opponent_oppId PRIMARY KEY (oppId)
)

CREATE TABLE [CurveBallConsultants.Tournament] (
	trnId CHAR(3) NOT NULL,
	trnName CHAR (50),
	CONSTRAINT pk_Tournament_trnId PRIMARY KEY (trnId)
)

CREATE TABLE [CurveBallConsultants.Location] (
	locId CHAR(3) NOT NULL,
	locCity CHAR(50),
	locState VARCHAR (20),
	CONSTRAINT pk_Location_locId PRIMARY KEY (locId)
)

CREATE TABLE [CurveBallConsultants.Match] (
	oppId CHAR(3) NOT NULL,
    locId CHAR(3) NOT NULL,
    mchDate DATE NOT NULL,
    mchTime TIME NOT NULL,
	mchType VARCHAR (7),
    runTerps INT,
    runOpponent INT,
    winLoss AS (
        CASE  
            WHEN runTerps - runOpponent > 0 THEN 'W'
            WHEN runTerps - runOpponent < 0 THEN 'L'
            ELSE 'D'
        END
    ),
	trnId CHAR(3),
	ssnYear CHAR(4),
	CONSTRAINT pk_Match_oppId_locId_mchDate_mchTime PRIMARY KEY (oppId, locId, mchDate, mchTime),
	CONSTRAINT fk_Match_oppId FOREIGN KEY (oppId)
		REFERENCES [CurveballConsultants.Opponent] (oppId)
		ON DELETE NO ACTION ON UPDATE CASCADE,
	CONSTRAINT fk_Match_locId FOREIGN KEY (locId)
		REFERENCES [CurveballConsultants.Location] (locId)
		ON DELETE NO ACTION ON UPDATE CASCADE,
	CONSTRAINT fk_Match_trnId FOREIGN KEY (trnId)
		REFERENCES [CurveBallConsultants.Tournament] (trnId)
		ON DELETE NO ACTION ON UPDATE CASCADE
);

