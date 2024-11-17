ALTER TABLE "CreditCard" ADD "NewCvv" TEXT;
UPDATE "CreditCard" SET "NewCvv" = "Cvv"::TEXT;
ALTER TABLE "CreditCard" DROP COLUMN "Cvv";
ALTER TABLE "CreditCard" RENAME COLUMN "NewCvv" TO "Cvv";
