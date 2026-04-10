CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "BlogPosts" (
    "Id" uuid NOT NULL,
    "Title" character varying(255) NOT NULL,
    "Slug" character varying(255) NOT NULL,
    "Content" text NOT NULL,
    "Thumbnail" text,
    "AuthorName" character varying(100) NOT NULL,
    "IsPublished" boolean NOT NULL,
    "PublishedAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BlogPosts" PRIMARY KEY ("Id")
);

CREATE TABLE "Categories" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Slug" character varying(100) NOT NULL,
    "Description" text,
    "ImageUrl" text,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE "Stores" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Address" character varying(500) NOT NULL,
    "Phone" character varying(20),
    "OpenTime" interval NOT NULL,
    "CloseTime" interval NOT NULL,
    "GoogleMapUrl" text,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_Stores" PRIMARY KEY ("Id")
);

CREATE TABLE "Products" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Slug" character varying(200) NOT NULL,
    "Description" text,
    "ImageUrl" text,
    "IsActive" boolean NOT NULL,
    "IsFeatured" boolean NOT NULL,
    "CategoryId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProductTags" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "TagName" character varying(50) NOT NULL,
    CONSTRAINT "PK_ProductTags" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProductTags_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProductVariants" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "SizeName" character varying(10) NOT NULL,
    "Temperature" character varying(20) NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "IsAvailable" boolean NOT NULL,
    CONSTRAINT "PK_ProductVariants" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProductVariants_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug")
VALUES ('11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.488916Z', 'Premium roasted coffee', NULL, TRUE, 'Coffee', 'coffee');
INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug")
VALUES ('22222222-2222-2222-2222-222222222222', TIMESTAMPTZ '2026-04-07T15:57:39.488954Z', 'Freshly brewed tea', NULL, TRUE, 'Tea', 'tea');
INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug")
VALUES ('33333333-3333-3333-3333-333333333333', TIMESTAMPTZ '2026-04-07T15:57:39.488954Z', 'Fruit smoothies', NULL, TRUE, 'Smoothies', 'smoothies');
INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug")
VALUES ('44444444-4444-4444-4444-444444444444', TIMESTAMPTZ '2026-04-07T15:57:39.488954Z', 'Delicious cakes', NULL, TRUE, 'Cakes', 'cakes');
INSERT INTO "Categories" ("Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug")
VALUES ('55555555-5555-5555-5555-555555555555', TIMESTAMPTZ '2026-04-07T15:57:39.488955Z', 'Quick bites', NULL, TRUE, 'Snacks', 'snacks');

INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10', '33333333-3333-3333-3333-333333333333', TIMESTAMPTZ '2026-04-07T15:57:39.493867Z', NULL, NULL, TRUE, FALSE, 'Strawberry Smoothie', 'strawberry-smoothie', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', '11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.493828Z', NULL, NULL, TRUE, FALSE, 'Espresso', 'espresso', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', '11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.493865Z', NULL, NULL, TRUE, FALSE, 'Americano', 'americano', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', '11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.493866Z', NULL, NULL, TRUE, FALSE, 'Latte', 'latte', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', '11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.493866Z', NULL, NULL, TRUE, FALSE, 'Cappuccino', 'cappuccino', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', '11111111-1111-1111-1111-111111111111', TIMESTAMPTZ '2026-04-07T15:57:39.493866Z', NULL, NULL, TRUE, FALSE, 'Mocha', 'mocha', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6', '22222222-2222-2222-2222-222222222222', TIMESTAMPTZ '2026-04-07T15:57:39.493866Z', NULL, NULL, TRUE, FALSE, 'Green Tea', 'green-tea', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7', '22222222-2222-2222-2222-222222222222', TIMESTAMPTZ '2026-04-07T15:57:39.493867Z', NULL, NULL, TRUE, FALSE, 'Black Tea', 'black-tea', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8', '22222222-2222-2222-2222-222222222222', TIMESTAMPTZ '2026-04-07T15:57:39.493867Z', NULL, NULL, TRUE, FALSE, 'Peach Tea', 'peach-tea', NULL);
INSERT INTO "Products" ("Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt")
VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9', '33333333-3333-3333-3333-333333333333', TIMESTAMPTZ '2026-04-07T15:57:39.493867Z', NULL, NULL, TRUE, FALSE, 'Mango Smoothie', 'mango-smoothie', NULL);

CREATE UNIQUE INDEX "IX_BlogPosts_Slug" ON "BlogPosts" ("Slug");

CREATE UNIQUE INDEX "IX_Categories_Slug" ON "Categories" ("Slug");

CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");

CREATE UNIQUE INDEX "IX_Products_Slug" ON "Products" ("Slug");

CREATE INDEX "IX_ProductTags_ProductId" ON "ProductTags" ("ProductId");

CREATE INDEX "IX_ProductVariants_ProductId" ON "ProductVariants" ("ProductId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260407155740_Init', '10.0.5');

COMMIT;

START TRANSACTION;
CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "FullName" text NOT NULL,
    "Role" text NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

UPDATE "Categories" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.289833Z'
WHERE "Id" = '11111111-1111-1111-1111-111111111111';

UPDATE "Categories" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.289873Z'
WHERE "Id" = '22222222-2222-2222-2222-222222222222';

UPDATE "Categories" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.289873Z'
WHERE "Id" = '33333333-3333-3333-3333-333333333333';

UPDATE "Categories" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.289873Z'
WHERE "Id" = '44444444-4444-4444-4444-444444444444';

UPDATE "Categories" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.289874Z'
WHERE "Id" = '55555555-5555-5555-5555-555555555555';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.294881Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.29484Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.294879Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.294879Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.294879Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.294879Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.29488Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.29488Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.29488Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8';

UPDATE "Products" SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T16:33:07.29488Z'
WHERE "Id" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9';

INSERT INTO "Users" ("Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Role")
VALUES ('99999999-9999-9999-9999-999999999999', TIMESTAMPTZ '2026-04-07T16:33:07.299805Z', 'admin@coffeeshop.com', 'System Admin', TRUE, '$2a$11$q2LzNjZBwt754ssEkedFN.dNj4FT7PWJrYsnRmMAgwFl4MWGECg9i', 'Admin');

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260407163308_AddUserTable', '10.0.5');

COMMIT;

