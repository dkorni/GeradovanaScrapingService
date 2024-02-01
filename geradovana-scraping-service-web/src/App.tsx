import { useEffect, useState } from "react";
import "./App.css";
import Header from "./components/Header";
import ListGroup from "./components/ListGroup";
import ReturnButton from "./components/ReturnButton";
import ProductCategorySummary from "./components/ProductCategorySummary";
import Spinner from "./components/Spinner";
import ProductCategorySummaryContainer from "./components/ProductCategorySummaryContainer";

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

    fetchSummary(item, "");
  };

  const handleSelectSubCategoryItem = (item: string, index: number) => {
    fetchSummary(currentCategory, item);
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
  const [fetching, SetFetching] = useState(false);

  const [summaries, setSummaries] = useState(
    [] as ProductCategorySummaryProps[]
  );

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

  const fetchSummary = async (category: string, subCategory: string) => {
    let apiCallUrl =
      subCategory == null
        ? apiUrl + "productcategories/summaries?category=" + category
        : apiUrl +
          "productcategories/summaries?category=" +
          category +
          "&subcategory=" +
          subCategory;

    SetFetching(true);
    const result = await fetch(apiCallUrl);
    result.json().then((jsonData) => {
      console.log(jsonData);
      setSummaries(jsonData);
      SetFetching(false);
    });
  };

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
                    onSelectItem={handleSelectSubCategoryItem}
                  ></ListGroup>
                </>
              )}
            </div>

            {summaries.length > 0 && !fetching ? (
              <ProductCategorySummaryContainer>
                {summaries.map((summary) => (
                  <ProductCategorySummary
                    type={summary.type}
                    amount={summary.amount}
                    averagePrice={summary.averagePrice}
                  ></ProductCategorySummary>
                ))}
              </ProductCategorySummaryContainer>
            ) : fetching ? (
              <div className="d-flex align-items-center justify-content-center flex-grow-1">
                <Spinner></Spinner>
              </div>
            ) : (
              <div className="d-flex align-items-center justify-content-center flex-grow-1">
                <h1 className="text-center">
                  Choose category to see details...
                </h1>
              </div>
            )}
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
