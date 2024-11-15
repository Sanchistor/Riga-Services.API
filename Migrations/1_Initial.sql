-- Create Users table with a capitalized name
CREATE TABLE "Users" (
                         "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                         "FirstName" TEXT NOT NULL,
                         "LastName" TEXT NOT NULL,
                         "Email" TEXT NOT NULL,
                         "Phone" TEXT NOT NULL,
                         "Password" TEXT NOT NULL,
                         "Role" INTEGER NOT NULL,
                         "Created" TIMESTAMPTZ NOT NULL,
                         "Updated" TIMESTAMPTZ NOT NULL,
                         "Balance" DOUBLE PRECISION NOT NULL DEFAULT 0.0
);

-- Create BusData table
CREATE TABLE "BusData" (
                           "Id" UUID PRIMARY KEY,
                           "BusNumber" INTEGER NOT NULL,
                           "BusCode" INTEGER NOT NULL,
                           "Type" INTEGER NOT NULL
);

-- Create TicketsInfo table
CREATE TABLE "TicketsInfo" (
                               "Id" UUID PRIMARY KEY,
                               "TicketType" INTEGER NOT NULL,
                               "Price" DOUBLE PRECISION NOT NULL
);

-- Create UserTickets table with foreign keys to BusData, TicketsInfo, and Users
CREATE TABLE "UserTickets" (
                               "Id" UUID PRIMARY KEY,
                               "Number" INTEGER NOT NULL,
                               "StartDate" TIMESTAMPTZ NULL,
                               "DueTime" TIMESTAMPTZ NULL,
                               "TicketsId" UUID NOT NULL,
                               "BusDataId" UUID NULL,
                               "UserId" UUID NOT NULL,
                               "Valid" BOOLEAN NOT NULL DEFAULT TRUE,
                               CONSTRAINT "FK_UserTickets_BusData" FOREIGN KEY ("BusDataId") REFERENCES "BusData"("Id") ON DELETE CASCADE,
                               CONSTRAINT "FK_UserTickets_TicketsInfo" FOREIGN KEY ("TicketsId") REFERENCES "TicketsInfo"("Id") ON DELETE CASCADE,
                               CONSTRAINT "FK_UserTickets_Users" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Create indexes on foreign key columns in UserTickets
CREATE INDEX "IX_UserTickets_BusDataId" ON "UserTickets" ("BusDataId");
CREATE INDEX "IX_UserTickets_TicketsId" ON "UserTickets" ("TicketsId");
CREATE INDEX "IX_UserTickets_UserId" ON "UserTickets" ("UserId");

