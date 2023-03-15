import { AfterViewInit, Component, OnInit, ViewChildren } from '@angular/core';
import  jspdf from 'jspdf';
import 'jspdf-autotable';
import html2canvas from 'html2canvas';
import { MatTable } from '@angular/material/table';

@Component({
  selector: 'app-pdfexport',
  templateUrl: './pdfexport.component.html',
  styleUrls: ['./pdfexport.component.scss']
})
export class PdfexportComponent implements OnInit {

  datasource:any;
  displayedColumns: string[] = ['Manufacturer', 'Description', 'Price', 'Quantity','Image'];
  @ViewChildren(MatTable) table: MatTable<string>;
  
  productdata: product[] = [
    {
      "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=1",
      "Manufacturer": "A630 - asdfasdfasdfasfConic Tub Chair",
      "Description": "4-6 weeks",
      "Price": "$1185.60",
      "Quantity": "1",
      "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="
    },
    {
      "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2",
      "Manufacturer": "A631 - asdfasdfasdfasfConic Tub Chair",
      "Description": "4-6 weeks",
      "Price": "$1185.60",
      "Quantity": "1",
      "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="
    },
    {
      "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=3",
      "Manufacturer": "A632 - asdfasdfasdfasfConic Tub Chair",
      "Description": "4-6 weeks",
      "Price": "$1185.60",
      "Quantity": "1",
      "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="

    }
  ]

  constructor() { }

  ngOnInit(): void {
    this.datasource = this.productdata;
  }


  head = [['ID', 'Country', 'Rank', 'Capital']]

  data = [
    [1, 'Finland', 7.632, 'Helsinki','http://maps.google.com/mapfiles/ms/icons/blue.png'],
    [2, 'Norway', 7.594, 'Oslo','http://maps.google.com/mapfiles/ms/icons/blue.png'],
    [3, 'Denmark', 7.555, 'Copenhagen','http://maps.google.com/mapfiles/ms/icons/blue.png'],
 
    // [1, 'Finland', 7.632, 'Helsinki','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // [2, 'Norway', 7.594, 'Oslo','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // [3, 'Denmark', 7.555, 'Copenhagen','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // // [4, 'Iceland', 7.495, 'Reykjavík','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // [5, 'Switzerland', 7.487, 'Bern','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // [9, 'Sweden', 7.314, 'Stockholm','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
    // [73, 'Belarus', 5.483, 'Minsk','data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=='],
  ]

  createPdf() {
    var doc = new jspdf();

    var imgData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg==";
    
    doc.setFontSize(18);
    doc.text('My PDF Table', 11, 8);
    doc.setFontSize(11);
    doc.setTextColor(100);


    (doc as any).autoTable({
      head: this.head,
      body: this.data,
      theme: 'plain',
      didDrawCell: data => {
        debugger;
        //  console.log('column index-',data.column.index);
        //  console.log('row-index-',data.row.index);
         if (data.section === 'body' && data.column.index === 4) {
           debugger;
         // console.log(data);
          var img = new Image();
         // image.src = imgData;
         img.src = data.cell.raw;
          var dim = data.cell.height - data.cell.padding('vertical');
         doc.addImage(img,data.cell.x,data.cell.y,dim,dim);
         }
        // if(data.column.index === 4)
        // {
        //   debugger;
        //   //console.log(data.cell);
        //    console.log(data.column.index);
        //    console.log(data.row.index);
          
        //   // var dim = data.cell.height - data.cell.padding('vertical');
        //   // //var textPos = cell.textPos;
        //   // //var imgdata = this.data[data.row.index][data.column.index].toString();
        //   // var imgdata = this.data[0][4].toString();
        //   // //doc.addImage(imgdata, data.cell.x,  data.cell.y, dim, dim);

        // }
      }
    })

    // Open PDF document in new tab
    //doc.output('dataurlnewwindow')

    //doc.addImage(imgData,50,50,150,76);

    // Download PDF document  
    doc.save('table.pdf');
  }

  generatePdf(tableId: string){
    debugger;

//     let DATA = document.getElementById(tableId);
    
// html2canvas(DATA).then(canvas => {
    
//     let fileWidth = 208;
//     let fileHeight = canvas.height * fileWidth / canvas.width;
    
//     const FILEURI = canvas.toDataURL('image/jpeg')
//     let PDF = new jspdf('p', 'mm', 'a4');
//     let position = 0;
//     PDF.addImage(FILEURI, 'JPEG', 0, position, fileWidth, fileHeight)
    
//     PDF.save('angular-demo.pdf');
// });
    var doc = new jspdf('p', 'mm', "a0");
    // var width = doc.getTextWidth('Text');
    // console.log(width);
    doc.setFontSize(10);
    (doc as any).autoTable({      
    html: this.table.renderRows, 
   // html:document.getElementById(tableId),       
    // bodyStyles: {minCellHeight: 15,minCellWidth: 10},
    // styles: { overflow: 'linebreak', cellWidth: 'wrap' },
    columnStyles: { text: { cellWidth: '10' } },
    didDrawCell: function(data) {
      debugger;
      console.log(data.column.index);
      //document.getElementById(tableId).innerHTML;
      // if (data.column.index === 5 && data.cell.section === 'body') {
      //   debugger;
      //    var td = data.cell.raw;
      //    var img = td.getElementsByTagName('img')[0];
      //    var dim = data.cell.height - data.cell.padding('vertical');
      //    //var textPos = data.cell.textPos;
      //   // doc.addImage(img.src, data.cell.x,  data.cell.y, 10, 10);
      // }
    }
  });

     doc.save("mytable.pdf");
  }

}

export interface product {
  Image: string
  Manufacturer: string
  Description: string
  Price: string
  Quantity: string
  imgBase64:any
}