-- =====================================================================
--  COFFEE SHOP DATABASE -- SUPABASE FINAL SCRIPT
--  Du an: CoffeeShop Backend (.NET 10 / EF Core / PostgreSQL)
--  Tong hop tu toan bo Migrations + Seed Data
--
--  HUONG DAN:
--    1. Mo Supabase Dashboard → SQL Editor
--    2. Paste toan bo noi dung file nay
--    3. Nhan "Run" (hoac Ctrl+Enter)
--    4. Script tu DROP bang cu → tao lai → seed data → migration history
--
--  Script nay la IDEMPOTENT (chay nhieu lan khong loi)
-- =====================================================================


-- =====================================================================
-- BUOC 1: DON DEP (dung thu tu FK de tranh loi)
-- =====================================================================

DROP TABLE IF EXISTS "ProductTags"           CASCADE;
DROP TABLE IF EXISTS "ProductVariants"       CASCADE;
DROP TABLE IF EXISTS "Products"              CASCADE;
DROP TABLE IF EXISTS "Categories"            CASCADE;
DROP TABLE IF EXISTS "BlogPosts"             CASCADE;
DROP TABLE IF EXISTS "Stores"                CASCADE;
DROP TABLE IF EXISTS "Users"                 CASCADE;
DROP TABLE IF EXISTS "__EFMigrationsHistory" CASCADE;


-- =====================================================================
-- BUOC 2: EXTENSION
-- =====================================================================

CREATE EXTENSION IF NOT EXISTS "pgcrypto";   -- cung cap gen_random_uuid()


-- =====================================================================
-- BUOC 3: BANG EF MIGRATIONS HISTORY
-- =====================================================================

CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId"    varchar(150) NOT NULL,
    "ProductVersion" varchar(32)  NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


-- =====================================================================
-- BUOC 4: TAO CAC BANG CHINH
-- =====================================================================

-- 4.1 Categories
CREATE TABLE "Categories" (
    "Id"          uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Name"        varchar(100) NOT NULL,
    "Slug"        varchar(100) NOT NULL,
    "Description" text,
    "ImageUrl"    text,
    "IsActive"    boolean      NOT NULL DEFAULT TRUE,
    "CreatedAt"   timestamptz  NOT NULL DEFAULT now(),
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

-- 4.2 Products
CREATE TABLE "Products" (
    "Id"          uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Name"        varchar(200) NOT NULL,
    "Slug"        varchar(200) NOT NULL,
    "Description" text,
    "ImageUrl"    text,
    "IsActive"    boolean      NOT NULL DEFAULT TRUE,
    "IsFeatured"  boolean      NOT NULL DEFAULT FALSE,
    "CategoryId"  uuid         NOT NULL,
    "CreatedAt"   timestamptz  NOT NULL DEFAULT now(),
    "UpdatedAt"   timestamptz,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

-- 4.3 ProductVariants  (SizeName: S/M/L  |  Temperature: Hot/Cold/Both)
CREATE TABLE "ProductVariants" (
    "Id"          uuid          NOT NULL DEFAULT gen_random_uuid(),
    "ProductId"   uuid          NOT NULL,
    "SizeName"    varchar(10)   NOT NULL,
    "Temperature" varchar(20)   NOT NULL,
    "Price"       numeric(18,2) NOT NULL,
    "IsAvailable" boolean       NOT NULL DEFAULT TRUE,
    CONSTRAINT "PK_ProductVariants" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProductVariants_Products_ProductId"
        FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

-- 4.4 ProductTags  (TagName: BEST SELLER / NEW / SEASONAL / ...)
CREATE TABLE "ProductTags" (
    "Id"        uuid        NOT NULL DEFAULT gen_random_uuid(),
    "ProductId" uuid        NOT NULL,
    "TagName"   varchar(50) NOT NULL,
    CONSTRAINT "PK_ProductTags" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProductTags_Products_ProductId"
        FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

-- 4.5 BlogPosts
CREATE TABLE "BlogPosts" (
    "Id"          uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Title"       varchar(255) NOT NULL,
    "Slug"        varchar(255) NOT NULL,
    "Content"     text         NOT NULL,
    "Thumbnail"   text,
    "AuthorName"  varchar(100) NOT NULL DEFAULT '',
    "IsPublished" boolean      NOT NULL DEFAULT FALSE,
    "PublishedAt" timestamptz,
    "CreatedAt"   timestamptz  NOT NULL DEFAULT now(),
    CONSTRAINT "PK_BlogPosts" PRIMARY KEY ("Id")
);

-- 4.6 Stores  (OpenTime/CloseTime luu kieu interval = TimeSpan cua C#)
CREATE TABLE "Stores" (
    "Id"           uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Name"         varchar(200) NOT NULL,
    "Address"      varchar(500) NOT NULL,
    "Phone"        varchar(20),
    "OpenTime"     interval     NOT NULL DEFAULT '08:00:00',
    "CloseTime"    interval     NOT NULL DEFAULT '22:00:00',
    "GoogleMapUrl" text,
    "IsActive"     boolean      NOT NULL DEFAULT TRUE,
    CONSTRAINT "PK_Stores" PRIMARY KEY ("Id")
);

-- 4.7 Users  (Role: Admin | User  |  PasswordHash: BCrypt)
CREATE TABLE "Users" (
    "Id"                     uuid         NOT NULL DEFAULT gen_random_uuid(),
    "Email"                  varchar(255) NOT NULL,
    "PasswordHash"           text         NOT NULL,
    "FullName"               text         NOT NULL DEFAULT '',
    "Role"                   text         NOT NULL DEFAULT 'User',
    "RefreshToken"           text,
    "RefreshTokenExpiryTime" timestamptz,
    "IsActive"               boolean      NOT NULL DEFAULT TRUE,
    "CreatedAt"              timestamptz  NOT NULL DEFAULT now(),
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);


-- =====================================================================
-- BUOC 5: INDEXES
-- =====================================================================

CREATE UNIQUE INDEX "IX_Categories_Slug"         ON "Categories"     ("Slug");
CREATE UNIQUE INDEX "IX_Products_Slug"            ON "Products"       ("Slug");
CREATE        INDEX "IX_Products_CategoryId"      ON "Products"       ("CategoryId");
CREATE        INDEX "IX_ProductVariants_ProductId" ON "ProductVariants" ("ProductId");
CREATE        INDEX "IX_ProductTags_ProductId"    ON "ProductTags"    ("ProductId");
CREATE UNIQUE INDEX "IX_BlogPosts_Slug"           ON "BlogPosts"      ("Slug");
CREATE UNIQUE INDEX "IX_Users_Email"              ON "Users"          ("Email");


-- =====================================================================
-- BUOC 6: SEED DATA
-- =====================================================================

-- 6.1 Categories (5 danh muc)
INSERT INTO "Categories" ("Id", "Name", "Slug", "Description", "IsActive", "CreatedAt") VALUES
    ('11111111-1111-1111-1111-111111111111', 'Coffee',    'coffee',    'Premium roasted coffee', TRUE, '2026-04-07T00:00:00Z'),
    ('22222222-2222-2222-2222-222222222222', 'Tea',       'tea',       'Freshly brewed tea',     TRUE, '2026-04-07T00:00:00Z'),
    ('33333333-3333-3333-3333-333333333333', 'Smoothies', 'smoothies', 'Fruit smoothies',        TRUE, '2026-04-07T00:00:00Z'),
    ('44444444-4444-4444-4444-444444444444', 'Cakes',     'cakes',     'Delicious cakes',        TRUE, '2026-04-07T00:00:00Z'),
    ('55555555-5555-5555-5555-555555555555', 'Snacks',    'snacks',    'Quick bites',            TRUE, '2026-04-07T00:00:00Z');

-- 6.2 Products (10 san pham)
INSERT INTO "Products" ("Id", "CategoryId", "Name", "Slug", "IsActive", "IsFeatured", "CreatedAt") VALUES
    -- Coffee (5)
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', '11111111-1111-1111-1111-111111111111', 'Espresso',            'espresso',            TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', '11111111-1111-1111-1111-111111111111', 'Americano',           'americano',           TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', '11111111-1111-1111-1111-111111111111', 'Latte',               'latte',               TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', '11111111-1111-1111-1111-111111111111', 'Cappuccino',          'cappuccino',          TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', '11111111-1111-1111-1111-111111111111', 'Mocha',               'mocha',               TRUE, FALSE, '2026-04-07T00:00:00Z'),
    -- Tea (3)
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6', '22222222-2222-2222-2222-222222222222', 'Green Tea',           'green-tea',           TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7', '22222222-2222-2222-2222-222222222222', 'Black Tea',           'black-tea',           TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8', '22222222-2222-2222-2222-222222222222', 'Peach Tea',           'peach-tea',           TRUE, FALSE, '2026-04-07T00:00:00Z'),
    -- Smoothies (2)
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9', '33333333-3333-3333-3333-333333333333', 'Mango Smoothie',      'mango-smoothie',      TRUE, FALSE, '2026-04-07T00:00:00Z'),
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10', '33333333-3333-3333-3333-333333333333', 'Strawberry Smoothie', 'strawberry-smoothie', TRUE, FALSE, '2026-04-07T00:00:00Z');

-- 6.3 ProductVariants (25 bien the, don vi: VND)
INSERT INTO "ProductVariants" ("Id", "ProductId", "SizeName", "Temperature", "Price", "IsAvailable") VALUES
    -- Espresso
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', 'S', 'Hot',  35000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', 'M', 'Hot',  45000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', 'L', 'Hot',  55000, TRUE),
    -- Americano
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', 'S', 'Hot',  35000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', 'M', 'Hot',  45000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', 'M', 'Cold', 50000, TRUE),
    -- Latte
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', 'S', 'Hot',  40000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', 'M', 'Hot',  50000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', 'M', 'Cold', 55000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', 'L', 'Cold', 65000, TRUE),
    -- Cappuccino
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', 'M', 'Hot',  50000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', 'L', 'Hot',  60000, TRUE),
    -- Mocha
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', 'M', 'Hot',  55000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', 'M', 'Cold', 60000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', 'L', 'Cold', 70000, TRUE),
    -- Green Tea
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6', 'M', 'Hot',  35000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6', 'M', 'Cold', 40000, TRUE),
    -- Black Tea
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7', 'M', 'Hot',  30000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7', 'M', 'Cold', 35000, TRUE),
    -- Peach Tea
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8', 'M', 'Cold', 45000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8', 'L', 'Cold', 55000, TRUE),
    -- Mango Smoothie
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9', 'M', 'Cold', 55000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9', 'L', 'Cold', 65000, TRUE),
    -- Strawberry Smoothie
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10', 'M', 'Cold', 55000, TRUE),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10', 'L', 'Cold', 65000, TRUE);

-- 6.4 ProductTags (6 tag)
INSERT INTO "ProductTags" ("Id", "ProductId", "TagName") VALUES
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', 'BEST SELLER'),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', 'BEST SELLER'),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', 'BEST SELLER'),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', 'NEW'),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8', 'NEW'),
    (gen_random_uuid(), 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9', 'SEASONAL');

-- 6.5 Admin User
--  Email   : admin@coffeeshop.com
--  Password: Admin@123   (BCrypt hash $2a$11$...)
INSERT INTO "Users" ("Id", "Email", "PasswordHash", "FullName", "Role", "RefreshToken", "RefreshTokenExpiryTime", "IsActive", "CreatedAt") VALUES
    (
        '99999999-9999-9999-9999-999999999999',
        'admin@coffeeshop.com',
        '$2a$11$q2LzNjZBwt754ssEkedFN.dNj4FT7PWJrYsnRmMAgwFl4MWGECg9i',
        'System Admin',
        'Admin',
        NULL,
        NULL,
        TRUE,
        '2026-04-07T00:00:00Z'
    );


-- =====================================================================
-- BUOC 7: EF MIGRATIONS HISTORY
--  (Giup EF Core nhan dien DB da duoc migrate day du,
--   khong chay lai migration khi app khoi dong)
-- =====================================================================

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
    ('20260407155740_Init',                        '10.0.5'),
    ('20260407163308_AddUserTable',                '10.0.5'),
    ('20260408141314_AddRefreshTokenToUser',       '10.0.5'),
    ('20260409060725_UpdateSeedDataToHashPassword','10.0.5');


-- =====================================================================
-- KIEM TRA NHANH SAU KHI CHAY (ket qua mong doi):
--   Categories         : 5
--   Products           : 10
--   ProductVariants    : 25
--   ProductTags        : 6
--   BlogPosts          : 0
--   Stores             : 0
--   Users              : 1
--   __EFMigrationsHistory: 4
-- =====================================================================

SELECT 'Categories'            AS "Table", COUNT(*) AS "Rows" FROM "Categories"
UNION ALL
SELECT 'Products',               COUNT(*) FROM "Products"
UNION ALL
SELECT 'ProductVariants',        COUNT(*) FROM "ProductVariants"
UNION ALL
SELECT 'ProductTags',            COUNT(*) FROM "ProductTags"
UNION ALL
SELECT 'BlogPosts',              COUNT(*) FROM "BlogPosts"
UNION ALL
SELECT 'Stores',                 COUNT(*) FROM "Stores"
UNION ALL
SELECT 'Users',                  COUNT(*) FROM "Users"
UNION ALL
SELECT '__EFMigrationsHistory',  COUNT(*) FROM "__EFMigrationsHistory";
