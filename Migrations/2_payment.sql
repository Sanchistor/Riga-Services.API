CREATE table "CreditCard" (
                              "Id" UUID PRIMARY KEY,
                              "UserId" UUID NOT NULL,
                              "CardNum" INTEGER not null,
                              "Date" TEXT NOT NULL,
                              "Cvv" INTEGER NOT NULL,
                              CONSTRAINT "FK_CreditCard_Users" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
)


