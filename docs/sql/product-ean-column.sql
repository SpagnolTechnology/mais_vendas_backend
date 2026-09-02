-- Adicionar coluna Ean em Products (rodar em cada tenant mais_vendas_{CNPJ})
ALTER TABLE "Products"
    ADD COLUMN IF NOT EXISTS "Ean" VARCHAR(14) NULL;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Products_Ean"
    ON "Products" ("Ean")
    WHERE "Ean" IS NOT NULL;

-- Rollback (se necessário):
-- DROP INDEX IF EXISTS "IX_Products_Ean";
-- ALTER TABLE "Products" DROP COLUMN IF EXISTS "Ean";
