using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Denizthai.Migrations
{
    public partial class CategorieIdNullableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add column if not exists (nullable)
            migrationBuilder.Sql(@"ALTER TABLE ""Tours"" ADD COLUMN IF NOT EXISTS ""CategorieId"" integer;");

            // Create index if not exists
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON c.relnamespace = n.oid
        WHERE c.relname = 'IX_Tours_CategorieId'
    ) THEN
        CREATE INDEX ""IX_Tours_CategorieId"" ON ""Tours"" (""CategorieId"");
    END IF;
END
$$;
");

            // Drop old FK only if exists (safe)
            migrationBuilder.Sql(@"ALTER TABLE ""Tours"" DROP CONSTRAINT IF EXISTS ""FK_Tours_Categories_CategorieId"";");

            // Add FK with ON DELETE SET NULL if not exists
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint c
        JOIN pg_class t ON c.conrelid = t.oid
        JOIN pg_namespace n ON t.relnamespace = n.oid
        WHERE c.conname = 'FK_Tours_Categories_CategorieId'
          AND t.relname = 'Tours'
    ) THEN
        ALTER TABLE ""Tours""
        ADD CONSTRAINT ""FK_Tours_Categories_CategorieId""
        FOREIGN KEY (""CategorieId"") REFERENCES ""Categories"" (""Id"") ON DELETE SET NULL;
    END IF;
END
$$;
");
        }

    }
}
