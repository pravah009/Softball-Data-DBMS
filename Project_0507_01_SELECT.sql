USE BUDT703_Project_0507_01

-- What are the top five teams against whom Maryland Softball won the most matches in the given range of years?
SELECT TOP 5 o.oppName, COUNT(*) as 'Maryland SoftBall Wins'
FROM [CurveBallConsultants.Match] m
JOIN [CurveBallConsultants.Opponent] o ON m.oppId = o.oppId
WHERE m.winLoss = 'W'
GROUP BY o.oppName
ORDER BY 'Maryland SoftBall Wins' DESC;

-- What are the top five teams against whom Maryland Softball lost the most matches in the given range of years?
SELECT TOP 5 o.oppName, COUNT(*) as 'Maryland SoftBall Losses'
FROM [CurveBallConsultants.Match] m
JOIN [CurveBallConsultants.Opponent] o ON m.oppId = o.oppId
WHERE m.winLoss = 'L'
GROUP BY o.oppName
ORDER BY 'Maryland SoftBall Losses' DESC;

-- What are the top five matches where Maryland Softball dominated the opposition in the given range of years?
SELECT TOP 5 o.oppName, m.mchDate, m.runTerps, m.runOpponent
FROM [CurveBallConsultants.Match] m
JOIN [CurveBallConsultants.Opponent] o ON m.oppId = o.oppId
WHERE m.winLoss = 'W'
ORDER BY (m.runTerps - m.runOpponent) DESC;

-- What is the longest winning streak and corresponding year for Maryland Softball?
SELECT MAX(StreakLength) as LongestWinningStreak, Streaks.ssnYear
FROM (
    SELECT ROW_NUMBER() OVER (ORDER BY m.mchDate) - ROW_NUMBER() OVER (PARTITION BY m.winLoss ORDER BY m.mchDate) as StreakGroup,
           COUNT(*) as StreakLength, m.ssnYear
    FROM [CurveBallConsultants.Match] m
    WHERE winLoss = 'W'
    GROUP BY m.ssnYear, m.mchDate, m.winLoss
) AS Streaks
GROUP BY Streaks.ssnYear;

-- What is the longest losing streak and corresponding year for Maryland Softball?
SELECT MAX(StreakLength) as LongestLosingStreak, Streaks.ssnYear
FROM (
    SELECT ROW_NUMBER() OVER (ORDER BY m.mchDate) - ROW_NUMBER() OVER (PARTITION BY m.winLoss ORDER BY m.mchDate) as StreakGroup,
           COUNT(*) as StreakLength, m.ssnYear
    FROM [CurveBallConsultants.Match] m
    WHERE m.winLoss = 'L'
    GROUP BY m.ssnYear, m.mchDate, m.winLoss
) AS Streaks
GROUP BY Streaks.ssnYear;

-- Which location has hosted the most matches in the given range of years?
SELECT TOP 1 l.locCity, l.locState, COUNT(*) as MatchesHosted
FROM [CurveBallConsultants.Match] m
JOIN [CurveBallConsultants.Location] l ON m.locId = l.locId
GROUP BY l.locCity, l.locState
ORDER BY MatchesHosted DESC;

-- How many Home games has Maryland lost?
SELECT COUNT(*) as HomeGamesLost
FROM [CurveBallConsultants.Match] m
WHERE m.mchType = 'Home' AND m.winLoss = 'L';

-- How many Away games has Maryland won?
SELECT COUNT(*) as AwayGamesWon
FROM [CurveBallConsultants.Match] m
WHERE m.mchType = 'Away' AND m.winLoss = 'W';

-- How many Neutral games has Maryland won?
SELECT COUNT(*) as NeutralGamesWon
FROM [CurveBallConsultants.Match] m
WHERE m.mchType = 'Neutral' AND m.winLoss = 'W';
