import * as React from 'react';
import Categories from './Categories';
import CategoryTodos from './CategoryTodos';

export default () => (
    <div className="row">
        <div className="col-2">
            <Categories />
        </div> 
        <div className="col-10">
            <CategoryTodos/>
        </div>
  </div>
);
