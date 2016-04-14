<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="SCM.YH">
    <SCM.YH>
      <xsl:apply-templates select="status" />
    </SCM.YH>
  </xsl:template>

  <xsl:template match="status">
    <Status>
      <xsl:attribute name="Value">
        <xsl:value-of select="value" />
      </xsl:attribute>
      <xsl:attribute name="Manage">
        <xsl:value-of select="manage" />
      </xsl:attribute>
    </Status>
  </xsl:template>

</xsl:stylesheet>