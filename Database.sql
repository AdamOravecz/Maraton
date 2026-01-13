CREATE DATABASE maraton
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_hungarian_ci;

USE maraton;

CREATE TABLE futok (
  fid     INT NOT NULL,          -- rajtszám (PK)
  fnev    VARCHAR(100) NOT NULL,  -- futó neve
  szulev  INT NOT NULL,           -- születési év
  szulho  INT NOT NULL,           -- születési hónap
  csapat  INT NOT NULL,           -- csapat azonosító
  ffi     BOOLEAN NOT NULL,       -- futó neme (1=férfi, 0=nő)

  CONSTRAINT pk_futok PRIMARY KEY (fid)
) ENGINE=InnoDB;

CREATE TABLE eredmenyek (
  futo INT NOT NULL,     -- futó rajtszáma (FK a futok.fid-re)
  kor  INT NOT NULL,     -- hanyadik kör
  ido  INT NOT NULL,     -- köridő (másodperc)

  CONSTRAINT pk_eredmenyek PRIMARY KEY (futo, kor),
  CONSTRAINT fk_eredmenyek_futok
    FOREIGN KEY (futo)
    REFERENCES futok (fid)
    ON DELETE CASCADE
    ON UPDATE CASCADE
) ENGINE=InnoDB;


INSERT INTO eredmenyek (futo, kor, ido)
VALUES (1044, 4, 15765)
ON DUPLICATE KEY UPDATE ido = VALUES(ido);


SELECT fnev, szulev
FROM futok
WHERE ffi = 0
ORDER BY fnev ASC;