-- =============================================================================
-- Proposta pública: campos na tabela tenant + catálogo admin
-- Rodar no banco ADMIN: mais_vendas_admin
-- Rodar em cada banco TENANT: mais_vendas_{CNPJ}
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1) BANCO ADMIN (mais_vendas_admin)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS "ProposalAuthentication" (
    "ProposalUuid" varchar(36) NOT NULL,
    "CompanyCnpj" varchar(14) NOT NULL,
    CONSTRAINT "PK_ProposalAuthentication" PRIMARY KEY ("ProposalUuid")
);

-- -----------------------------------------------------------------------------
-- 2) BANCO TENANT (mais_vendas_{CNPJ}) — tabela Proposals
-- -----------------------------------------------------------------------------
ALTER TABLE "Proposals"
    ADD COLUMN IF NOT EXISTS "ProposalUuid" uuid NULL;

UPDATE "Proposals"
SET "ProposalUuid" = gen_random_uuid()
WHERE "ProposalUuid" IS NULL;

ALTER TABLE "Proposals"
    ALTER COLUMN "ProposalUuid" SET NOT NULL;

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Proposals_ProposalUuid"
    ON "Proposals" ("ProposalUuid");

-- -----------------------------------------------------------------------------
-- 3) Sincronizar catálogo admin para propostas já existentes (rodar no ADMIN)
--    Ajuste o CNPJ abaixo antes de executar.
-- -----------------------------------------------------------------------------
-- INSERT INTO "ProposalAuthentication" ("ProposalUuid", "CompanyCnpj")
-- SELECT p."ProposalUuid"::text, '00000000000000'
-- FROM dblink(
--     'host=... dbname=mais_vendas_00000000000000 user=... password=...',
--     'SELECT "ProposalUuid" FROM "Proposals"'
-- ) AS p("ProposalUuid" uuid)
-- ON CONFLICT ("ProposalUuid") DO NOTHING;
