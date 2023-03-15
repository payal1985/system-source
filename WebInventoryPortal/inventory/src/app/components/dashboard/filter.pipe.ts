import { Pipe, PipeTransform } from '@angular/core';
import { inventory } from 'src/app/models/inventory';


@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {
  transform(inventoryList: inventory[], find: string): inventory[] {
    //debugger;
    if(!inventoryList) return [];
    if(!find) return inventoryList;
    find = find.toLowerCase();
    return search(inventoryList, find);
   }
}

function search(entries: inventory[], search: string) {
//debugger;
  search = search.toLowerCase();
  
  return entries.filter(function(data){
    return JSON.stringify(data).toLowerCase().includes(search);
});
}

//https://stackblitz.com/edit/stackoverflowcom-a-60857864-6433166-dpaump?file=src%2Fapp%2Fapp.component.html
//https://stackblitz.com/edit/angular-material-search-posts2?file=src%2Fapp%2Fsearch-posts.component.ts
//https://remotestack.io/angular-custom-filter-search-pipe-example-tutorial/