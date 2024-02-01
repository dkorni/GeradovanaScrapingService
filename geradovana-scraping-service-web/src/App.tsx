import { useEffect, useState } from "react";
import "./App.css";
import Header from "./components/Header";
import ListGroup from "./components/ListGroup";
import ReturnButton from "./components/ReturnButton";
import Spinner from "./components/Spinner";

function App() {
  const items = ["New York", "Kyiv", "Paris", "London", "Tokyo"];

  const handleSelectCategoryItem = (item: string, index: number) => {
    var categoryData = serviceResponseCategories[index];
    var subCategories = categoryData.subCategories;
    if (subCategories != null) {
      SetListSubCategories(subCategories);
      SetSubcategoryEnabled(true);
      SetCurrentCategory(categoryData.name);
    }
  };

  const handleOnSubItemListReturnButton = () => {
    SetSubcategoryEnabled(false);
  };

  const [wasLoaded, SetWasLoaded] = useState(false);
  const [subcategoryEnabled, SetSubcategoryEnabled] = useState(false);
  const [serviceResponseCategories, SetServiceResponseCategories] = useState(
    [] as ProductCategory[]
  );
  const [listCategories, SetListCategories] = useState([] as string[]);
  const [listSubCategories, SetListSubCategories] = useState([] as string[]);
  const [currentCategory, SetCurrentCategory] = useState("");

  const apiUrl = "http://localhost:5247/";

  useEffect(() => {
    const fetchData = async () => {
      const result = await fetch(apiUrl + "productcategories");
      result.json().then((jsonData: ProductCategory[]) => {
        console.log(jsonData);
        SetServiceResponseCategories(jsonData);
        SetListCategories(jsonData.map((x) => x.name));
        SetWasLoaded(true);
      });
    };
    fetchData();
  }, []);

  return (
    <>
      {wasLoaded ? (
        <>
          <Header></Header>
          <div className="d-flex">
            <div className="p-2 w-10">
              {!subcategoryEnabled ? (
                <ListGroup
                  heading="Categories"
                  items={listCategories}
                  onSelectItem={handleSelectCategoryItem}
                ></ListGroup>
              ) : (
                <>
                  <ReturnButton
                    onClick={handleOnSubItemListReturnButton}
                  ></ReturnButton>
                  <ListGroup
                    heading={currentCategory}
                    items={listSubCategories}
                    onSelectItem={() => {}}
                  ></ListGroup>
                </>
              )}
            </div>
            <div className="d-flex align-items-center justify-content-center flex-grow-1">
              <h1 className="text-center">Choose category to see details...</h1>
            </div>
          </div>
        </>
      ) : (
        <div className="d-flex align-items-center justify-content-center flex-grow-1 vh-100">
          <Spinner></Spinner>
        </div>
      )}
    </>
  );
}

export default App;
