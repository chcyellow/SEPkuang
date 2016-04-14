<xsl:stylesheet version="1.0"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
 xmlns:user="urn:my-scripts"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" > 
 
<xsl:template match="/">
  <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:x="urn:schemas-microsoft-com:office:excel"
    xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:html="http://www.w3.org/TR/REC-html40">
    <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
      <Author>ghtn</Author>
      <LastAuthor>xl</LastAuthor>
      <Created>2010-01-29T08:54:46Z</Created>
      <LastSaved>2010-01-29T08:59:56Z</LastSaved>
      <Company>微软中国</Company>
      <Version>11.9999</Version>
    </DocumentProperties>
    <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
      <WindowHeight>8670</WindowHeight>
      <WindowWidth>11715</WindowWidth>
      <WindowTopX>240</WindowTopX>
      <WindowTopY>30</WindowTopY>
      <ProtectStructure>False</ProtectStructure>
      <ProtectWindows>False</ProtectWindows>
    </ExcelWorkbook>
    <Styles>
      <Style ss:ID="Default" ss:Name="Normal">
        <Alignment ss:Vertical="Center"/>
        <Borders/>
        <Font ss:FontName="宋体" x:CharSet="134" ss:Size="12"/>
        <Interior/>
        <NumberFormat/>
        <Protection/>
      </Style>
      <Style ss:ID="s21">
        <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
        <Borders>
          <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
        </Borders>
        <Font ss:FontName="宋体" x:CharSet="134" ss:Size="12" ss:Bold="1"/>
      </Style>
      <Style ss:ID="s22">
        <Alignment ss:Vertical="Center" ss:WrapText="1"/>
        <Borders>
          <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
        </Borders>
      </Style>
      <Style ss:ID="s28">
        <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
        <Borders>
          <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
        </Borders>
        <Font ss:FontName="宋体" x:CharSet="134" ss:Size="22" ss:Bold="1"/>
      </Style>
      <Style ss:ID="s29">
        <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
        <Borders>
          <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
          <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
        </Borders>
        <Font ss:FontName="宋体" x:CharSet="134" ss:Size="11" ss:Bold="1"/>
      </Style>
    </Styles>
    <xsl:apply-templates/>
  </Workbook>
</xsl:template>


<xsl:template match="/*">
  <Worksheet>
  <xsl:attribute name="ss:Name">
  <xsl:value-of select="local-name(/*/*)" />
  </xsl:attribute>
	  <Table ss:ExpandedColumnCount="13"  x:FullColumns="1" 
   x:FullRows="1" ss:DefaultColumnWidth="54" ss:DefaultRowHeight="14.25">
      <Column ss:AutoFitWidth="0" ss:Width="60.75"/>
      <Column ss:AutoFitWidth="0" ss:Width="59.25"/>
      <Column ss:AutoFitWidth="0" ss:Width="59.5"/>
      <Column ss:AutoFitWidth="0" ss:Width="100.5"/>
      <Column ss:AutoFitWidth="0" ss:Width="79.75"/>
      <Column ss:AutoFitWidth="0" ss:Width="113.25"/>
      <Column ss:AutoFitWidth="0" ss:Width="66.75"/>
      <Column ss:AutoFitWidth="0" ss:Width="83.25"/>
      <Column ss:AutoFitWidth="0" ss:Width="65.25"/>
		  <Row ss:AutoFitHeight="0" ss:Height="32.25">
			  <Cell ss:MergeAcross="8" ss:StyleID="s28">
				  <Data ss:Type="String">走动计划信息报表</Data>
			  </Cell>
		  </Row>
		  <Row>
			  <xsl:for-each select="*[position() = 1]/*">
				  <Cell ss:StyleID="s21">
					  <Data ss:Type="String">
						  <xsl:value-of select="local-name()" />
					  </Data>
				  </Cell>
			  </xsl:for-each>
		  </Row>
		  <xsl:apply-templates/>
	  </Table>
  </Worksheet>
</xsl:template>


<xsl:template match="/*/*">
  <Row>
    <xsl:apply-templates/>
  </Row>
</xsl:template>


<xsl:template match="/*/*/*">
  <Cell ss:StyleID="s22"><Data ss:Type="String">
    <xsl:value-of select="." />
  </Data></Cell>
</xsl:template>


</xsl:stylesheet>
