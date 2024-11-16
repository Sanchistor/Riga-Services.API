-- Step 1: Add a new column with the correct type
ALTER TABLE "CreditCard" ADD "NewCardNum" TEXT;

-- Step 2: Copy data from the old column to the new column
UPDATE "CreditCard" SET "NewCardNum" = "CardNum"::TEXT;

-- Step 3: Drop the old column
ALTER TABLE "CreditCard" DROP COLUMN "CardNum";

-- Step 4: Rename the new column to the original column name
ALTER TABLE "CreditCard" RENAME COLUMN "NewCardNum" TO "CardNum";
